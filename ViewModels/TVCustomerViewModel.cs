using System;
using AssetManager.DragDrop;

namespace AssetManager.ViewModels
{
    public class TVCustomerViewModel : TreeViewItemViewModel, IDropable
    {
        Models.TVCustomerModel _customer;

        public TVCustomerViewModel(Models.TVCustomerModel _parentcustomer) : base(null, true)
        {
            Customer = _parentcustomer;
            IsExpanded = false;
            IsSelected = false;
        }

        protected override void LoadChildren()
        {
            FullyObservableCollection<Models.AssetModel> _assets = SQLiteQueries.GetCustomerChildAssets(Customer.ID);
            foreach (Models.AssetModel am in _assets)
            {
                base.Children.Add(new TVAssetViewModel(am, null));
            }
        }
        
        public Models.TVCustomerModel Customer
        {
            get { return _customer; }
            set { SetField(ref _customer, value); }
        }
        //Visibility isvisible;
        //public Visibility Visibilty
        //{
        //    get { return isvisible; }
        //    set { SetField(ref isvisible, value); }
        //}

        #region IDropable Members

        /// <summary>
        /// Only TVAssetViewModel can be dropped
        /// </summary>
        Type IDropable.DataType
        {
            get { return typeof(TVAssetViewModel); }
        }

        /// <summary>
        /// Drop data into this ViewModel
        /// </summary>
        void IDropable.Drop(object data, int index)
        {
            if (data is TVAssetViewModel source)
            {
                if (source.Asset.ParentAssetID == 0 && Customer.ID == source.Asset.CustomerID) //if dragged and dropped yourself, don't need to do anything
                    return;

                SQLiteQueries.UpdateParentAssetID(source.Asset.ID, 0, Customer.ID);
                ViewModels.AssetTreeExViewModel.MoveAsset(source.Asset.ID, 0, Customer.ID);

            }
        }

        #endregion

    }
}
