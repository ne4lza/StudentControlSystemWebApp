﻿using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace EYOkulProjectWebUI.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult Index(DateTime startDate, DateTime endDate,int selectDate)
        {
            using (var context = new EYOkulDbContext())
            
            {
                switch (selectDate)
                {
                    case 0:
                        startDate = DateTime.Today;
                        endDate = DateTime.Today;
                        break;
                    case 1:
                        startDate = DateTime.Today.AddDays(-1);
                        endDate = DateTime.Today.AddDays(-1);
                        break;
                    default:
                        break;
                }
                var startDateParam = new SqlParameter("@fdate", startDate);
                var endDateParam = new SqlParameter("@ldate", endDate);
                var userName = new SqlParameter("@username","Yunus");

                var tr = context.TBL_TRANSACTIONS
                                          .FromSqlRaw("EXEC sp_Transaction @fdate, @ldate,@username",
                                                      startDateParam, endDateParam,userName)
                                          .ToList();

                return View("Index", tr);
            }
        }
    }
}