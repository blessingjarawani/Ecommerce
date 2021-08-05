using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Models.DTO;
using BoookStoreDatabase2.DAL.Entities;
using BoookStoreDatabase2.WEB.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace BoookStoreDatabase2.WEB.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CartController : BaseController
    {
      
    }
}