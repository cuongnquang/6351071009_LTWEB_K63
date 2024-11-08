using System;
using System.Linq;

namespace _6351071009_LTWEB_K63.Models
{
    public class Giohang
    {
        // Tham chiếu đến DbContext của Entity Framework
        private QLBansachEntities data;

        // Các thuộc tính của giỏ hàng
        public int iMasach { get; set; }
        public string sTensach { get; set; }
        public string sAnhbia { get; set; }
        public double dDongia { get; set; }
        public int iSoluong { get; set; }

        // Tính tổng tiền cho sản phẩm trong giỏ hàng
        public double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }

        // Constructor nhận vào tham số là một đối tượng DbContext
        public Giohang(int Masach, QLBansachEntities data)
        {
            iMasach = Masach;
            SACH sach = data.SACHes.Single(n => n.Masach == iMasach);
            sTensach = sach.Tensach;
            sAnhbia = sach.Anhbia;
            dDongia = (double)sach.Giaban;  // Explicit cast from decimal to double
            iSoluong = 1;
        }


        // Nếu muốn thêm sản phẩm vào giỏ hàng sau này, có thể thêm phương thức
        public void TangSoLuong()
        {
            iSoluong++;
        }
    }
}
