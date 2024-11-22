//using System;
//using System.ComponentModel.DataAnnotations;

//namespace HospitalManagements.Models
//{
//    // Custom validation attribute for future date validation
//    public class FutureDateAttribute : ValidationAttribute
//    {
//        public override bool IsValid(object value)
//        {
//            DateTime date;
//            if (value is DateTime)
//            {
//                date = (DateTime)value;
//            }
//            else if (value is string)
//            {
//                if (DateTime.TryParse((string)value, out date))
//                {
//                    return date.Date >= DateTime.Today;
//                }
//                return false;
//            }
//            else
//            {
//                return false;
//            }

//            return date.Date >= DateTime.Today;
//        }
//    }
//}




////using System.ComponentModel.DataAnnotations;

////namespace HospitalManagements.Models
////{
////    public class FutureDateAttribute  :ValidationAttribute
////    {


////        public override bool IsValid(object value)
////        {
////            if (value is DateTime dateTime)
////            {
////                return dateTime.Date >= DateTime.Now.Date;
////            }
////            return false;
////        }



////    }
////}
