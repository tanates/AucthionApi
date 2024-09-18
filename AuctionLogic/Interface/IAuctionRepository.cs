

using AuctionEntity.DTO.Req;
using AuctionEntity.Entity;
using AuctionEntity.Model.Context;
using AuctionEntity.Model.DTO;
using AuctionEntity.Model.DTO.ErrorDTO;
using AuctionEntity.Model.DTO.Req;
using ErrorHendler.sql;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AuctionEntity.Interface
{
    public interface IAuctionRepository
    {

        public Task<BaseDto<AuctionDTO>> Creat<T>(T reqAuc);
        public Task<BaseDto<AuctionDTO>> GetById(Guid aucId);
        public Task<IEnumerable<BaseDto<AuctionDTO>>> GetAll();
        public Task<BaseDto<AuctionDTO>> Delete(Guid idObject);
    }


    public class AuctionRepository : IAuctionRepository
    {

        private readonly AuctionDbContext auctionDbContex;

        public AuctionRepository(AuctionDbContext auctionDbContex)
        {
            this.auctionDbContex=auctionDbContex;
        }
//scope AuctiDTO ?
        public async Task<BaseDto<AuctionDTO>> Creat<T>(T reqAuc)
        {
            if (reqAuc is AuctionDTO reqFromGateway)
            {
                try
                {
                    var res = await SqlHandler.ExecuteDbOperationAsync(async () =>
                    {
                        var res = await auctionDbContex.Auction.AddAsync(AucEntity.ConvertToAucEntity(reqFromGateway));
                        await auctionDbContex.SaveChangesAsync();
                        return res.Entity;
                    });
                    return new BaseDto<AuctionDTO> { Data = AuctionDTO.ConvertToAuctionDTO(res) , ErrorDto =  null};
                }
                catch (Exception ex)
                {

                    return new BaseDto<AuctionDTO> {  ErrorDto =  new ErrorHandlerDTO<AuctionDTO>(null  ,false , "Ошибка при создании аукциона"+ex.Message ) };
                }

                

            };
            throw new ArgumentException("Неверный тип аргумента", nameof(reqAuc));
        }

        public async Task<BaseDto<AuctionDTO>> GetById(Guid aucId) 
        {

            var res = await SqlHandler.ExecuteDbOperationAsync(async   () =>
            {

                var resQ = await auctionDbContex.Auction.FirstOrDefaultAsync(i => i.Id == aucId) ?? new AucEntity();
                var aucDto = AuctionDTO.ConvertToAuctionDTO(resQ);
                return new BaseDto<AuctionDTO>()
                {
                   
                       Data = aucDto,
                       ErrorDto = new ErrorHandlerDTO<AuctionDTO>()
                 
                };


            });

           return res;
        }


        public async Task<IEnumerable<BaseDto<AuctionDTO>>> GetAll() 
        {
            var res = await SqlHandler.ExecuteDbOperationAsync(async () =>
            {
               
                var res = await auctionDbContex.Auction.ToListAsync();
                var auctionDtos =  res.Select(auc =>AuctionDTO.ConvertToAuctionDTO(auc));
                var baseDtos  = auctionDtos.Select(dto => new BaseDto<AuctionDTO> { Data = dto , ErrorDto =  new ErrorHandlerDTO<AuctionDTO>() });
                return baseDtos;
            });

            return res;
        }

        public Task<BaseDto<AuctionDTO>> Delete(Guid idObject)
        {
            throw new NotImplementedException();
        }

  
    }


   
}
