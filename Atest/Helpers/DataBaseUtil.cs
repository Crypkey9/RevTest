using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Atest.Models
{
    public class DataBaseUtil
    {
        public static async Task<DBtestUser> GetUser(int ids)
        {
            try
            {
                dataEntities db = new dataEntities();

                var query = await (from info in db.DBtestUsers
                                   where info.Id == ids
                                   select new DBtestUser
                                   {
                                       Id = info.Id,
                                       FirstName = info.FirstName,
                                       Password = info.Password,
                                       Address = info.Address,
                                       BirthODate = info.BirthODate
                                   }).SingleAsync();



                return query;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        public static async Task<String> validateLogIn(String user1, String pass1)
        {
            try
            {
                dataEntities db = new dataEntities();

                var getU = await (from UserAcc in db.DBtestUsers
                                  where UserAcc.FirstName == user1 && UserAcc.Password == pass1
                                  select UserAcc).SingleOrDefaultAsync();

                if (getU != null)
                {
                    return "valid";
                }
                else
                {
                    return "invalid";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "Error";
            }
        }
    }
}