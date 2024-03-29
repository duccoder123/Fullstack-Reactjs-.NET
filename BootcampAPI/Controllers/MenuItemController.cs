﻿using BootcampAPI.Data;
using BootcampAPI.Models;
using BootcampAPI.Models.Dto;
using BootcampAPI.Service;
using BootcampAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BootcampAPI.Controllers
{
    [Route("api/MenuItem")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;
        private readonly ILocalService _localService;
        public MenuItemController(ApplicationDbContext db, ILocalService localService, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;

            _localService = localService;
            _response = new ApiResponse();
        }
        [HttpGet]
        public async Task<IActionResult> GetMenuItems()
        {
            _response.Result = _db.MenuItems.ToList();
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetMenuItem")]
        public async Task<IActionResult> GetMenuItem(int id)
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            MenuItem menuItem = _db.MenuItems.FirstOrDefault(p => p.Id == id);
            if (menuItem == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            _response.Result = menuItem;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        [HttpPost]
        [Authorize(Roles =SD.Role_Admin)]
        public async Task<ActionResult<ApiResponse>> CreateMenuItem([FromForm] MenuItemCreateDto menuItemCreateDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (menuItemCreateDto.File == null || menuItemCreateDto.File.Length == 0)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(menuItemCreateDto.File.FileName)}";
                    string imagePath = Path.Combine("C:\\Users\\HP\\Desktop\\LearnReact\\typescript\\typescript\\src\\Assets\\images");

                 
                    Directory.CreateDirectory(imagePath);

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    await menuItemCreateDto.File.CopyToAsync(fileStream); ;
                    MenuItem menuItemCreate = new()
                    {
                        Name = menuItemCreateDto.Name,
                        Price = menuItemCreateDto.Price,
                        Category = menuItemCreateDto.Category,
                        SpecialTag = menuItemCreateDto.SpecialTag,
                        Description = menuItemCreateDto.Description,
                        Image = await _localService.UploadBlob(fileName, menuItemCreateDto.File)
                    };
                    _db.MenuItems.Add(menuItemCreate);
                    _db.SaveChanges();
                    _response.Result = menuItemCreate;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("GetMenuItem", new { id = menuItemCreate.Id }, _response);
                }
                else
                {
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return Ok(_response);
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuItemUpdateDto menuItemUpdateDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(menuItemUpdateDto == null || id != menuItemUpdateDto.Id)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        return BadRequest();
                    }
                    MenuItem menuItem = _db.MenuItems.FirstOrDefault(x => x.Id == id);
                    if (menuItem == null)
                    {
                        _response.StatusCode = HttpStatusCode.NotFound;
                        _response.IsSuccess = false;
                        return BadRequest();
                    }
                    menuItem.Name = menuItemUpdateDto.Name;
                    menuItem.Description = menuItemUpdateDto.Description;
                    menuItem.Price = menuItemUpdateDto.Price;
                    menuItem.Category = menuItemUpdateDto.Category;
                    menuItem.SpecialTag = menuItemUpdateDto.SpecialTag;
                    if (menuItemUpdateDto.File != null & menuItemUpdateDto.File.Length > 0)
                    {
                        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(menuItemUpdateDto.File.FileName)}";
                  
                        menuItem.Image = await _localService.UploadBlob(fileName, menuItemUpdateDto.File);
                    }
                    _db.MenuItems.Update(menuItem);
                    _db.SaveChanges();
                    _response.Result = menuItemUpdateDto;
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return Ok(_response);
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return BadRequest();
                }
                MenuItem item = _db.MenuItems.FirstOrDefault(x => x.Id == id);
                if (item == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return BadRequest();
                }
                await _localService.DeleteBlob(item.Image.Split("/").Last());
                int miliseconds = 2000;
                Thread.Sleep(miliseconds);
                _db.MenuItems.Remove(item);
                _db.SaveChanges();
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return Ok(_response);
        }
    }
}