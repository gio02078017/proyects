namespace GrupoExito.Android.Adapters
{
    using GrupoExito.Entities;

    public interface IFilterAdapter
    {
        void OnCategoryChecked(ProductFilter category);
        void OnBrandChecked(ProductFilter brand);
    }
}