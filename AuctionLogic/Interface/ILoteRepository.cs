using AuctionEntity.DTO.Req;
using AuctionEntity.Entity;
using AuctionEntity.Model.Context;
using AuctionEntity.Model.DTO;
using AuctionEntity.Model.DTO.ErrorDTO;
using AuctionEntity.Model.DTO.Req;
using ErrorHendler.sql;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionLogic.Interface
{
    public interface ILoteRepository
    {
        public Task<BaseDto<LoteDTO>> Creat<T>(T reqAuc) ;
        public Task<BaseDto<LoteDTO>> GetById(Guid aucId);
        public Task<IEnumerable<BaseDto<LoteDTO>>> GetAll();
        public Task<BaseDto<LoteDTO>> DeleteAuc(Guid idObject);
    }

    public class LoteRepository : ILoteRepository
    {
        private readonly AuctionDbContext auctionDbContex;

        public LoteRepository(AuctionDbContext auctionDbContex)
        {
            this.auctionDbContex=auctionDbContex;
        }
        public async Task<BaseDto<LoteDTO>> Creat<T>(T reqAuc)
        {
            if (reqAuc is ReqFromGateway reqFromGateway)
            {
                Lote lote = new Lote()
                {
                    Description  = reqFromGateway.LoteDTO.Description, 
                    EndPrice = reqFromGateway.LoteDTO.EndPrice,
                    Id = reqFromGateway.LoteDTO.Id,
                    IdAuctione = reqFromGateway.LoteDTO.IdAuctione,
                    Name = reqFromGateway.LoteDTO.Name,
                    StartPrice = reqFromGateway.LoteDTO.StartPrice,
                };

                var res = await SqlHandler.ExecuteDbOperationAsync<BaseDto<LoteDTO>>(async () =>
                {
                    var res = await auctionDbContex.Lotes.AddAsync(lote);
                    await auctionDbContex.SaveChangesAsync();
                    return new BaseDto<LoteDTO>
                    {
                        Data = LoteDTO.ConvertToLoteDTO(res.Entity),
                        ErrorDto = new ErrorHandlerDTO<LoteDTO>()
                    };
                });

                return res;
            }
            return new BaseDto<LoteDTO> { Data  = new LoteDTO(), ErrorDto =  new ErrorHandlerDTO<LoteDTO>(default , false , "model is not valid") };
        }

        public Task<BaseDto<LoteDTO>> DeleteAuc(Guid idObject)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BaseDto<LoteDTO>>> GetAll() 
        {
            var res = await SqlHandler.ExecuteDbOperationAsync(async () =>
            {

                var lotes = await auctionDbContex.Lotes.ToListAsync();
                var loteDTO = lotes.Select(l => LoteDTO.ConvertToLoteDTO(l));
                // Преобразуем Lote в BaseDto<Lote>
                var baseDtos = loteDTO.Select(lote => new BaseDto<LoteDTO>
                {
                    Data = lote,
                    ErrorDto = new ErrorHandlerDTO<LoteDTO>() // Заполните ErrorDto по необходимости
                });

                return baseDtos;

            });

            return res;
        }

        public Task<BaseDto<LoteDTO>> GetById(Guid aucId) 
        {
            throw new NotImplementedException();
        }

       
    }
}
