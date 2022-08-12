/*
 * Class that defines the entries for the database and data sent in responses
 */

using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Name cannot be empty")] // Can impose rules on the post / put requests to validate entries
        public string Name { get; set; }

        [Required(ErrorMessage = "Must enter an address")]
        public string Address { get; set; }
    }
}

