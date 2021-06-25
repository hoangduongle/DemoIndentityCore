using DemoIndentityCore.Areas.Identity.Data;
using DemoIndentityCore.Data;
using DemoIndentityCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoIndentityCore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly DemoIndentityCoreContext _db;
        private readonly UserManager<DemoIndentityCoreUser> _userManager;

        public AccountController(DemoIndentityCoreContext db, UserManager<DemoIndentityCoreUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            var lstUser = _db.Users
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber
                }).ToList();
            return View(lstUser);
        }

        public ActionResult Create()
        {
            return View();
        }

        //POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                var user = new DemoIndentityCoreUser
                {
                    UserName = collection["UserName"],
                    Email = collection["Email"],
                    FirstName = collection["FirstName"],
                    LastName = collection["LastName"],
                    PhoneNumber = collection["PhoneNumber"],
                };
                var result = await _userManager.CreateAsync(user, "Abc**123");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //Get: AccountController/Details/S
        public ActionResult Details(string id)
        {
            var user_detail = _db.Users.Where(x => x.Id == id)
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber
                })
                .FirstOrDefault();
            return View(user_detail);
        }

        public ActionResult Edit(string id)
        {
            var user_detail = _db.Users.Where(x => x.Id == id)
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber
                })
                .FirstOrDefault();
            return View(user_detail);
        }

        //POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(string id, IFormCollection collection)
        {
            try
            {
                var user = _db.Users.Where(x => x.Id == id).FirstOrDefault();
                user.UserName = collection["UserName"];
                user.Email = collection["Email"];
                user.FirstName = collection["FirstName"];
                user.LastName = collection["LastName"];
                user.PhoneNumber = collection["PhoneNumber"];

                var result = await _userManager.UpdateAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> DeleteAsync(string id)
        {
            var user = _db.Users.Where(x => x.Id == id).FirstOrDefault();
            var result = await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

    }
}
