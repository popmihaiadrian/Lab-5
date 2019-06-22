﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab_2_webapi.Models
{
    public enum UserRole
    {
        Regular,
        User_Manager,
        Admin,
    }

    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [EnumDataType(typeof(UserRole))]
        public UserRole UserRole { get; set; }
        public DateTime CreatedAt { get; set; }
        public String CreatedBy { get; set; }
        public String CreatedByRole { get; set; }
    }
}
