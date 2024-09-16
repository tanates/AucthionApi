using AuctionEntity.Model.DTO.Req;
using AuctionEntity.Model.DTO;
using AuctionLogic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionLogic.Servisec
{
     public interface ILoteServices
    {
        public Task<BaseDto<LoteDTO>> DeleteEntity(Guid idObject);

        public  Task<BaseDto<LoteDTO>> GetEntity(Guid idObject);


        public Task<IEnumerable<BaseDto<LoteDTO>>> GetEntityAll();


        public Task<(bool isSuccess, string message)> SetEntity<T>(T modelReq) where T : class;
     
    }

    public class AuctionLote : ILoteServices
    {
        private readonly ILoteRepository _loteRepository;
        public AuctionLote(ILoteRepository acuctioneRepository)
        {
            _loteRepository = acuctioneRepository;
        }

        public Task<BaseDto<LoteDTO>> DeleteEntity(Guid idObject) 
        {
            throw new NotImplementedException();
        }

        public async Task<BaseDto<LoteDTO>> GetEntity(Guid idObject)
        {
            return await _loteRepository.GetById(idObject);

        }

        public async Task<IEnumerable<BaseDto<LoteDTO>>> GetEntityAll()
        {
            var res = await _loteRepository.GetAll();

            return res;

        }

        public async Task<(bool isSuccess, string message)> SetEntity<T>(T modelReq) where T : class
        {
            var res = await _loteRepository.Creat(modelReq);
            return (res.ErrorDto.Result, res.ErrorDto.Messange);
        }
    }
}
