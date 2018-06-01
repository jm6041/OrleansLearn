using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InfomationManager.Models;
using Orleans;
using InfomationManager.Abstractions;

namespace InfomationManager.WebClient.Controllers
{
    public class SystemUserController : Controller
    {
        private readonly IClusterClient _client;
        private readonly ISystemUserGrain _userGrain;
        public SystemUserController(IClusterClient client)
        {
            _client = client;
            _userGrain = _client.GetGrain<ISystemUserGrain>(Guid.NewGuid());
        }

        // GET: SystemUser
        public async Task<IActionResult> Index()
        {
            return View(await _userGrain.Get());
        }

        // GET: SystemUser/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemUser = await _userGrain.GetById(id.Value);
            if (systemUser == null)
            {
                return NotFound();
            }

            return View(systemUser);
        }

        // GET: SystemUser/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SystemUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Age")] SystemUser systemUser)
        {
            if (ModelState.IsValid)
            {
                systemUser.Id = Guid.NewGuid();
                await _userGrain.Add(systemUser);
                return RedirectToAction(nameof(Index));
            }
            return View(systemUser);
        }

        // GET: SystemUser/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemUser = await _userGrain.GetById(id.Value);
            if (systemUser == null)
            {
                return NotFound();
            }
            return View(systemUser);
        }

        // POST: SystemUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Email,Age")] SystemUser systemUser)
        {
            if (id != systemUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _userGrain.Update(systemUser);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(systemUser);
        }
    }
}
