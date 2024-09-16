using AuctionEntity.DTO.Req;
using AuctionEntity.Interface;
using AuctionEntity.Model.Context;
using AuctionEntity.Model.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AuctionLogic.Services
{
    public interface IAcuctioneServices
    {
        public Task <(bool isSuccess, string message)> SetEntity <T>(T modelReq) where T : class;
        public Task<BaseDto<AuctionDTO>> GetEntity(Guid idObject);

        public Task<IEnumerable<BaseDto<AuctionDTO>>> GetEntityAll();

        public Task<BaseDto<AuctionDTO>> DeleteEntity(Guid idObject);
        
        
     
    }

    public class AuctionServices : IAcuctioneServices
    {
      
        private readonly IServiceProvider _serviceProvider;
 
        public AuctionServices(IServiceProvider serviceProvider )
        {
            
            _serviceProvider=serviceProvider;
        }

        public async Task<BaseDto<AuctionDTO>> DeleteEntity(Guid idObject) 
        {
            using var scope = _serviceProvider.CreateScope();
           
            
            var _acuctioneRepository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();
            var _logger = scope.ServiceProvider.GetRequiredService<ILogger<AuctionServices>>();

            var res = await _acuctioneRepository.Delete(idObject);
            _logger.LogInformation($"---Send to message {res}");
            
            if (!res.ErrorDto.Result)
            {
                _logger.LogInformation($"---Message not transfer: " + "\n"
                    +$"---Services return this error message : __{res.ErrorDto.Messange}__");
            }

            return res;     
        }

        public   async Task <BaseDto<AuctionDTO>> GetEntity(Guid  idObject) 
        {
            using var scope = _serviceProvider.CreateScope();

            var _acuctioneRepository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();

            var _logger = scope.ServiceProvider.GetRequiredService<ILogger<AuctionServices>>();


            var res = await _acuctioneRepository.GetById(idObject);
            if (res == null)
                throw new Exception("");


            if (!res.ErrorDto.Result)
            {
                _logger.LogInformation($"---Message not transfer: " + "\n"
                    +$"---Services return this error message : __{res.ErrorDto.Messange}__");
            }

            return res;

        }

        public async Task<IEnumerable<BaseDto<AuctionDTO>>> GetEntityAll() 
        {
            using var scope = _serviceProvider.CreateScope();

            var _acuctioneRepository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();

            var _logger = scope.ServiceProvider.GetRequiredService<ILogger<AuctionServices>>();

            var res = await _acuctioneRepository.GetAll();

            if (res == null)
            {
                var ret = res.Select(r => new BaseDto<AuctionDTO>{ Data = null , ErrorDto = r.ErrorDto});

                return ret;
            }

             
            if (res!= null)
            {
                _logger.LogInformation($"---Message not transfer: " + "\n"
                    +$"---Services return this error message : __res is null__");
            }


            return res;
        }

        public async Task<(bool isSuccess, string message)> SetEntity<T>(T modelReq) where T : class
        {
            using var scope = _serviceProvider.CreateScope();
            
            var _acuctioneRepository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();

            var _logger = scope.ServiceProvider.GetRequiredService<ILogger<AuctionServices>>();

            if (modelReq is AuctionDTO req)
            {
                
                var res = await _acuctioneRepository.Creat(req);
                
                if (!res.ErrorDto.Result)
                {
                    _logger.LogInformation($"---Message not transfer: " + "\n"
                        +$"---Services return this error messnge : __{res.ErrorDto.Messange}__");
                }

                return (res.ErrorDto.Result , res.ErrorDto.Messange);
               
            }


            return (false , "fff");

        }



    }


}
