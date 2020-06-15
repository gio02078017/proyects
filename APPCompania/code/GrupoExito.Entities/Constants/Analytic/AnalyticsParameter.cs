namespace GrupoExito.Entities.Constants.Analytic
{
    public static class AnalyticsParameter
    {
        public static string UserId { get => "id_usuario"; }
        public static string UserType { get => "tipo-usuario"; }

        ////Products
        public static string Description { get => "description"; }
        public static string PLU { get => "plu"; }
        public static string Percentage { get => "percentage"; }

        ////Types lists products
        public static string CustomerProducts { get => "Recomendados"; }
        public static string DiscountProducts { get => "descuentos"; }

        ////Steps pay
        public static string StepPayOne { get => "1"; }
        public static string StepPayTwo { get => "2"; }
        public static string StepPayThree { get => "3"; }
        public static string StepPayFour { get => "4"; }
    }
}
