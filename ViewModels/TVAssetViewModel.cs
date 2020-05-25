using System;
using AssetManager.DragDrop;

namespace AssetManager.ViewModels
{
    public class TVAssetViewModel : TreeViewItemViewModel, IDropable, IDragable
    {
        Models.AssetModel _asset;
     
        public TVAssetViewModel(TVAssetViewModel _parentasset) : base(_parentasset, true)
        {
            Asset = _parentasset.Asset;
        }

        public TVAssetViewModel(Models.AssetModel _asset, TVAssetViewModel _assetvm) : base(_assetvm, true)
        {
            Asset = _asset;
            IsExpanded = false;
            IsSelected = false;
            //Asset.Label = GlobalClass.MakeLabel(Asset.AssetAreaID, Asset.AssetGroupID, Asset.LabelID);
        }

        protected override void LoadChildren()
        {
            FullyObservableCollection<Models.AssetModel> _assets = SQLiteQueries.GetChildAssets(Asset.ID);
            foreach (Models.AssetModel am in _assets)
                base.Children.Add(new TVAssetViewModel(am, this));
        }

        public Models.AssetModel Asset
        {
            get { return _asset; }
            set { SetField(ref _asset, value); }
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
            //if moving within customer, reassign the children to the 
            //level above first

            if (data is TVAssetViewModel source)
            {
                if (source.Asset.ID == Asset.ID || source.Asset.ParentAssetID == Asset.ID) //if dragged and dropped yourself, don't need to do anything
                    return;
                SQLiteQueries.UpdateParentAssetID(source.Asset.ID, Asset.ID, Asset.CustomerID);
                AssetTreeExViewModel.MoveAsset(source.Asset.ID, Asset.ID, Asset.CustomerID);

            }
        }

        #endregion

        #region IDragable Members

        /// <summary>
        /// Only TVAssetViewModel can be dragged
        /// </summary>
        Type IDragable.DataType
        {
            get
            {
                return typeof(TVAssetViewModel);
            }
        }

        /// <summary>
        /// Remove this TVAssetViewModel from the 
        /// TVAssetViewModel
        /// </summary>
        /// <param name="i"></param>
        void IDragable.Remove(object data)
        {

        }

        #endregion


    }
}
