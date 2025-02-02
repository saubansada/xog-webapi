﻿using System;
using System.Linq;
using XOG.AppCode.DAL;
using XOG.Helpers;
using XOG.Models;
using XOG.Util;

//namespace XOG.AppCode.Transformers
//{
//    public static class UserTransformer
//    {

//        public static object MapToUserModelList<T>(this IQueryable<AspNetUser> query, object obj = null, ListingType listType = ListingType.List)
//        {
//            if (query == null)
//            {
//                return null;
//            }

//            var _query = (IQueryable<object>)query;


//            if (typeof(Type) == typeof(UserViewModel))
//            {
//                _query = query.Select(model => new UserViewModel
//                {

//                    Id = model.Id,
//                    FirstName = model.FirstName,
//                    LastName = model.LastName,
//                    Email = model.Email,
//                    EmailConfirmed = model.EmailConfirmed,
//                    Mobile = model.PhoneNumber,
//                    AlternateMobileNumber = model.AlternateMobileNumber,
//                    PhoneNumberConfirmed = model.PhoneNumberConfirmed,
//                    LockedOutEnabled = model.LockoutEnabled,
//                    RegistrationDate = model.RegistrationDate,
//                    UserType = model.UserType,
//                    Roles = model.AspNetRoles.Select(i => i.Name).ToList()
//                });
//            }
//            else if (type == ModelType.OListItem && obj.isNullOrWholeNumber())
//            {
//                int id = obj.NullReverse();
//                return query.Select(model => new OListItem
//                {
//                    Text = model.UserName,
//                    Value = model.Id.ToString(),
//                    Selected = id != -1 ? model.Id.Equals(id) : false
//                }).ToList();
//            }

//            if (listType == ListingType.Queryable)
//            {
//                return _query;
//            }
//            else if (listType == ListingType.GridList && obj != null)
//            {
//                _query = _query.UpdateGridModelList((GridModel)obj);
//            }
//            return _query.ToList();
//        }


//        public static object TransformToUserModel<T>(this AspNetUser model, object obj = null)
//        {
            
//            if (typeof(T) == typeof(Address))
//            {
//                var Addresses = model.Addresses.Select(address => new
//                {
//                    address.Id,
//                    address.AddressOfUserId,
//                    address.AreaCode, 
//                    address.Country,
//                    address.GPS,  
//                    address.PhoneNumber
//                });

//                return new
//                {
//                    IsLoggedIn = true,
//                    model.Id,
//                    model.PhoneNumber,
//                    model.PhoneNumberConfirmed,
//                    FirstName = model.FirstName ?? "",
//                    LastName = model.LastName ?? "",
//                    model.Email,
//                    model.EmailConfirmed,
//                    Addresses = Addresses
//                };
//            }
//            //else if (type == ModelType.OListItem && obj.isNullOrWholeNumber())
//            //{
//            //    int id = obj.NullReverse();
//            //    return new OListItem
//            //    {
//            //        Text = model.Email,
//            //        Value = model.Id.ToString(),
//            //        Selected = id != -1 ? model.Id.Equals(id) : false
//            //    };
//            //}
//            else
//            {
//                return model;
//            }
//        }

//    }
//}