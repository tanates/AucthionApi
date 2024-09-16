﻿using AuctionEntity.DTO.Req;
using AuctionEntity.Interface;
using AuctionEntity.Model.DTO;
using AuctionEntity.Model.DTO.ErrorDTO;
using AuctionLogic.Services;
using AuctionLogic.Servisec;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AuctionLogic
{
    public  interface IAucSet
    {
        public Task<BaseDto<Dictionary<string, object>>> startAuction(AuctionDTO Model);
        public Task<Dictionary<string, object>> stopAuctionById(Guid id);
        public Task<Dictionary<string, object>> setPriceLote<T>(T Model);
        public Task<Dictionary<string, object>> stopAuctionWhereTimeIsNull(Guid id);
    }


    public class MainAuct : IAucSet
    {


        private readonly IServiceScopeFactory _serviceScopeFactory;
        public MainAuct(IServiceScopeFactory serviceScopeFactory)
        {

            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task<Dictionary<string, object>> setPriceLote<T>(T Model)
        {
            throw new NotImplementedException();
        }

        public async  Task<BaseDto<Dictionary<string, object>>> startAuction(AuctionDTO model)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var _acuctioneServisec = scope.ServiceProvider.GetRequiredService<IAcuctioneServices>();
                    

                    
                    if (model == null) throw new ArgumentNullException("model");

                    
                    var creat = await _acuctioneServisec.SetEntity(model);
                    if (!creat.isSuccess)
                    {
                        return new BaseDto<Dictionary<string, object>>
                        {

                            ErrorDto =  new ErrorHandlerDTO<Dictionary<string, object>>(new Dictionary<string, object>(), creat.isSuccess, creat.message)


                        };

                    }

                    return new BaseDto<Dictionary<string, object>>(new Dictionary<string, object> { { "message", creat.message }, { "result", creat.isSuccess } });

                }
            }
            catch (Exception ex)
            {
                return new BaseDto<Dictionary<string, object>>
                {

                    ErrorDto =  new ErrorHandlerDTO<Dictionary<string, object>>(new Dictionary<string, object>(), false, ex.Message)

                };
            }
        }

       

        public async Task<Dictionary<string , object>> stopAuctionById( Guid id )
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _acuctioneServisec = scope.ServiceProvider.GetRequiredService<IAcuctioneServices>();

                var res = await _acuctioneServisec.GetEntity(id);

                if (!res.ErrorDto.Result)
                {
                    return new Dictionary<string, object>
                {
                    {"error" , res.ErrorDto.Messange},
                    {"result" , false }
                };
                }
                return new Dictionary<string, object>
            {
                {"error" , null},
                {"result" , true},
                {"data" ,(BaseDto<AuctionDTO>)res}
            };
            }
        }

        public Task<Dictionary<string, object>> stopAuctionWhereTimeIsNull(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

