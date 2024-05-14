//using EFDatabaseModel.DbModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using ProjectBase.Repository.Transaction;
//using EFDatabaseModel.Contexts;
//using Microsoft.EntityFrameworkCore;
//using EFDatabaseModel.Repository;
//using EFDatabaseModel.Models;

//namespace LukurareBackend.Repositories.LoadAirtime
//{
//    public class LoadMeterRepository : EFDatabaseModelBaseRepository<PaymentModel>
//    {
//        public LoadMeterRepository(string currentModule)
//            : base(currentModule) { }

//        public async Task<ExecutionResult<IEnumerable<MfsCurrencyType>>> GetCurrency()
//        {
//            var result = new ExecutionResult<IEnumerable<MfsCurrencyType>>(
//                true,
//                "",
//                new HashSet<MfsCurrencyType>()
//            );
//            try
//            {
//                using (var context = new EFDatabaseModelDatabaseContext())
//                {
//                    var returnData = await context.MfsCurrencyTypes.ToListAsync();

//                    result.Result = returnData.Select(
//                        r =>
//                            new MfsCurrencyType
//                            {
//                                Id = r.Id,
//                                CurrencyCode = r.CurrencyCode,
//                                CurrencySymbol = r.CurrencySymbol,
//                                CurrencyName = r.CurrencyName
//                            }
//                    );
//                }
//            }
//            catch (Exception ex)
//            {
//                result = result.CopyFromException(ex);
//            }
//            return result;
//        }

//        public override Task<ExecutionResult<PaymentModel>> Add(
//            PaymentModel model,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            throw new NotImplementedException();
//        }

//        public override PaymentModel CloneModel(PaymentModel model)
//        {
//            throw new NotImplementedException();
//        }

//        public override Task<ExecutionResult<PaymentModel>> GetItem(
//            int id,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            throw new NotImplementedException();
//        }

//        public override Task<ExecutionResult<PaymentModel>> Update(
//            PaymentModel model,
//            EFDatabaseModelDatabaseContext context = null
//        )
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
