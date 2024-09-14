using AuctionEntity.DTO.Req;
using AuctionEntity.Entity;
using AuctionEntity.Interface;
using AuctionEntity.Model.DTO;
using AuctionEntity.Model.DTO.ErrorDTO;
using AuctionEntity.Model.DTO.Req;
using AuctionLogic.Interface;
using ErrorHendler.sql;
using System.Net.Http.Headers;
using System.Linq;
using MQConnection;
using Microsoft.Extensions.Configuration;

namespace AuctionLogic.Servisec
{
    public interface IAcuctioneServisec
    {
        public Task <(bool isSuccess, string message)> SetEntity <T>(T modelReq) where T : class;
        public Task<BaseDto<AuctionDTO>> GetEntity(Guid idObject);

        public Task<IEnumerable<BaseDto<AuctionDTO>>> GetEntityAll();

        public Task<BaseDto<AuctionDTO>> DeleteEntity(Guid idObject);
        
        
     
    }

    public class AuctionServisec : IAcuctioneServisec
    {
        private readonly IAuctionRepository _acuctioneRepository;
        private readonly IMessageProducer _messageProducer;
        public AuctionServisec(IAuctionRepository acuctioneRepository , IMessageProducer messageProducer)
        {
            _acuctioneRepository=acuctioneRepository;
            _messageProducer=messageProducer;
        }

        public async Task<BaseDto<AuctionDTO>> DeleteEntity(Guid idObject) 
        {

            
            var res = await _acuctioneRepository.Delete(idObject);
            await  _messageProducer.Send(, res);
            return res;
        }

        public   async Task <BaseDto<AuctionDTO>> GetEntity(Guid  idObject) 
        {

            var res = await _acuctioneRepository.GetById(idObject);
            if (res == null)
                throw new Exception("");

            return res;

            
        }

        public async Task<IEnumerable<BaseDto<AuctionDTO>>> GetEntityAll() 
        {
            var res = await _acuctioneRepository.GetAll();

            if (res == null)
            {
                var ret = res.Select(r => new BaseDto<AuctionDTO>{ Data = null , ErrorDto = r.ErrorDto});

                return ret;
            }

            return res;
        }

        public async Task<(bool isSuccess, string message)> SetEntity<T>(T modelReq) where T : class
        {
            
                if (modelReq is ReqFromGateway req)
                {
                
                    var res = await _acuctioneRepository.Creat(req.AuctionDTO);

                return (res.ErrorDto.Result , res.ErrorDto.Messange);
                }


            return (false , "fff");

        }



    }


}
