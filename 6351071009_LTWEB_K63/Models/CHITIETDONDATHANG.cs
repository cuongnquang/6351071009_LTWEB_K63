//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _6351071009_LTWEB_K63.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CHITIETDONDATHANG
    {
        public int MaDonHang { get; set; }
        public int Masach { get; set; }
        public Nullable<int> Soluong { get; set; }
        public Nullable<decimal> Dongia { get; set; }
    
        public virtual DONDATHANG DONDATHANG { get; set; }
        public virtual SACH SACH { get; set; }
    }
}
