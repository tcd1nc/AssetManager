using System;
using System.Linq;
using System.Windows;
using AssetManager.DragDrop;

namespace AssetManager.ViewModels
{
    public class TVItemVM : ViewModelBase, IDropable, IDragable
    {
        bool isExpanded;
        bool isSelected;
        bool isFiltered;
        bool isChecked;
        string label;
        int id;

        FullyObservableCollection<TVItemVM> children;
        TVItemVM parent;

        protected TVItemVM(TVItemVM parentNode)
        {
            Parent = parentNode;
            Children = new FullyObservableCollection<TVItemVM>();
        }

        protected TVItemVM(TVItemVM parentNode, bool loadchildren)
        {
            Parent = parentNode;
            Children = new FullyObservableCollection<TVItemVM>();
            if(loadchildren == true)
                LoadChildren();
        }

        protected TVItemVM()
        {
           
        }


        #region Properties

        FullyObservableCollection<TVItemVM> sourcecollection;
        public FullyObservableCollection<TVItemVM> SourceCollection
        {
            get { return sourcecollection; }
            set { SetField(ref sourcecollection, value);}
        }

        Visibility isvisible;
        public Visibility Visibility
        {
            get { return isvisible; }
            set { SetField(ref isvisible, value); }
        }

        public bool Expanded
        {
            get { return isExpanded; }
            set { SetField(ref isExpanded, value); }
        }

        public bool Selected
        {
            get { return isSelected; }
            set { SetField(ref isSelected, value); }
        }

        public bool Filtered
        {
            get { return isFiltered; }
            set { SetField(ref isFiltered, value); }
        }

        public bool Checked
        {
            get { return isChecked; }
            set { SetField(ref isChecked, value); }
        }

        public FullyObservableCollection<TVItemVM> Children
        {
            get { return children; }
            set { SetField(ref children, value); }
        }

        public TVItemVM Parent
        {
            get { return parent; }
            set { SetField(ref parent, value); }
        }

        public int ID
        {
            get { return id; }
            set { SetField(ref id, value); }
        }

        public string Label
        {
            get { return label; }
            set { SetField(ref label, value); }
        }

        #endregion

        #region Functions

        public void LoadChildren()
        {   
            if(SourceCollection != null)
            {               
                foreach (TVItemVM am in SourceCollection)
                    if(am.Parent.ID == this.ID)
                        Children.Add(new TVItemVM(am, true));
            }           
        }

        public void ClearChildren()
        {
            if(Children != null)            
                Children.Clear();            
        }
        
        public void AddChild()
        {
            TVItemVM newchild = new TVItemVM();
            Children.Add(newchild);
        }

        public void RemoveChild(TVItemVM child)
        {
            Children.Remove(child);
        }

        public void RemoveChild(int id)
        {
            TVItemVM child = Children.Where(x => x.ID == id).FirstOrDefault();
            if(child != null)
                Children.Remove(child);
        }

        #endregion
        
        #region IDropable Interface

        Type IDropable.DataType
        {
            get { return typeof(TVItemVM); }
        }

        /// <summary>
        /// Drop data into this ViewModel
        /// </summary>
        void IDropable.Drop(object data, int index)
        {
            //if moving within customer, reassign the children to the 
            //level above first

            if (data is TVItemVM source)
            {
                if (source == this || source.Parent == this.Parent) //if dragged and dropped yourself, don't need to do anything
                    return;
                

                //raise Dropped event

            }
        }


        #endregion

        #region IDragable Interface
        
        /// <summary>
        /// Only TVAssetViewModel can be dragged
        /// </summary>
        Type IDragable.DataType
        {
            get
            {
                return typeof(TVItemVM);
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
