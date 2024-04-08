﻿namespace Ecom.Core.Entities
{
	public class Address
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }


        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}