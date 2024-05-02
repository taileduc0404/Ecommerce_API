﻿using System.Runtime.Serialization;

namespace Ecom.Core.Entities.Orders
{
	public enum OrderStatus
	{
		[EnumMember(Value = "Pending")]
		Pending, 
		[EnumMember(Value = "Payment Reveived")]
		PaymentReveived, 
		[EnumMember(Value = "Payment Felid")]
		PaymentFelid
	}
}