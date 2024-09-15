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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQConnection;

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
      
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        public AuctionServisec(IServiceProvider serviceProvider,   ILoggerFactory logger)
        {
            _logger = logger.CreateLogger(typeof(AuctionServisec));
            _serviceProvider=serviceProvider;
        }

        public async Task<BaseDto<AuctionDTO>> DeleteEntity(Guid idObject) 
        {
            using var scope = _serviceProvider.CreateScope();
           
            var _messageProducer = scope.ServiceProvider.GetRequiredService<IMessageProducer>();
            var _acuctioneRepository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();
            var _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var confLsit = _configuration.GetSection("StartRabbitSettings");
            var con = scope.ServiceProvider.GetRequiredService<IRabbitConnection>();
            var res = await _acuctioneRepository.Delete(idObject);
            _logger.LogInformation($"---Send to message {res}");
            var sendRes = await  _messageProducer.Send(confLsit, res, con);
            if (!sendRes.IsSuccses)
            {
                _logger.LogInformation($"---Messange not transefer: " + "\n"
                    +$"---Servis return this error messange : __{sendRes.Message}__");
            }

            return new BaseDto<AuctionDTO>
            {

                ErrorDto =  new ErrorHandlerDTO<AuctionDTO> { Value = res.Data, Result = sendRes.IsSuccses, Messange = sendRes.Message }
            };        
        }

        public   async Task <BaseDto<AuctionDTO>> GetEntity(Guid  idObject) 
        {
            using var scope = _serviceProvider.CreateScope();

            var con = scope.ServiceProvider.GetRequiredService<IRabbitConnection>();
            var _messageProducer = scope.ServiceProvider.GetRequiredService<IMessageProducer>();
            var _acuctioneRepository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();
            var _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var confLsit = _configuration.GetSection("StartRabbitSettings");

           
            var res = await _acuctioneRepository.GetById(idObject);
            if (res == null)
                throw new Exception("");

            var sendRes = await _messageProducer.Send(confLsit, res, con);
            if (!sendRes.IsSuccses)
            {
                _logger.LogInformation($"---Messange not transefer: " + "\n"
                    +$"---Servis return this error messange : __{sendRes.Message}__");
            }

            return new BaseDto<AuctionDTO>
            {
                Data = res.Data,
                ErrorDto =  new ErrorHandlerDTO<AuctionDTO> { Value = res.Data, Result = sendRes.IsSuccses, Messange = sendRes.Message }
            };

        }

        public async Task<IEnumerable<BaseDto<AuctionDTO>>> GetEntityAll() 
        {
            using var scope = _serviceProvider.CreateScope();
            var con = scope.ServiceProvider.GetRequiredService<IRabbitConnection>();

            var _messageProducer = scope.ServiceProvider.GetRequiredService<IMessageProducer>();
            var _acuctioneRepository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();
            var _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var confLsit = _configuration.GetSection("StartRabbitSettings");
            var res = await _acuctioneRepository.GetAll();

            if (res == null)
            {
                var ret = res.Select(r => new BaseDto<AuctionDTO>{ Data = null , ErrorDto = r.ErrorDto});

                return ret;
            }

             var sendRes = await _messageProducer.Send(confLsit, res, con);
            if (!sendRes.IsSuccses)
            {
                _logger.LogInformation($"---Messange not transefer: " + "\n"
                    +$"---Servis return this error messange : __{sendRes.Message}__");
            }


            return res;
        }

        public async Task<(bool isSuccess, string message)> SetEntity<T>(T modelReq) where T : class
        {
            using var scope = _serviceProvider.CreateScope();
            var con = scope.ServiceProvider.GetRequiredService<IRabbitConnection>();
            var _messageProducer = scope.ServiceProvider.GetRequiredService<IMessageProducer>();
            var _acuctioneRepository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();
            var _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var confLsit = _configuration.GetSection("StartRabbitSettings");
            if (modelReq is ReqFromGateway req)
            {
                
                var res = await _acuctioneRepository.Creat(req.AuctionDTO);
                var sendRes = await _messageProducer.Send(confLsit, res, con);
                if (!sendRes.IsSuccses)
                {
                    _logger.LogInformation($"---Messange not transefer: " + "\n"
                        +$"---Servis return this error messange : __{sendRes.Message}__");
                }

                return (res.ErrorDto.Result , res.ErrorDto.Messange);
               
            }


            return (false , "fff");

        }



    }


}
