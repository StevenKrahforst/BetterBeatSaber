using System.Collections.Generic;
using System.IO;
using System.Reflection;

using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Components;

using HMUI;

using UnityEngine;

namespace BetterBeatSaber.UI; 

public abstract class ListView<T, TC> : TableView.IDataSource where TC : ListCell<T> {

    private static FieldInfo? CanSelectSelectedCellField => typeof(TableView).GetField("_canSelectSelectedCell", BindingFlags.Instance | BindingFlags.NonPublic);
    
    protected BetterBeatSaber BetterBeatSaber => BetterBeatSaber.Instance;

    public CustomListTableData? Table { get; private set; }
    
    public virtual IList<T> Items { get; protected set; } = new List<T>();

    public bool AllowReselection {
        get {
            if (Table == null)
                return false;
            return (bool?) CanSelectSelectedCellField?.GetValue(Table.tableView) ?? false;
        }
        set {
            if (Table != null)
                CanSelectSelectedCellField?.SetValue(Table.tableView, value);
        }
    }

    protected virtual float ItemCellSize { get; set; } = 14f;

    protected ListView(CustomListTableData table, bool reload = true) {
        Table = table;
        if(table.tableView != null)
            table.tableView.SetDataSource(this, reload);
    }
    
    public float CellSize() => ItemCellSize;

    public int NumberOfCells() => Items.Count;

    public TableCell CellForIdx(TableView tableView, int idx) {

        var item = Items[idx];

        var cell = (TC?) tableView.DequeueReusableCellForIdentifier(typeof(TC).Name);
        if (cell == null)
            cell = CreateCell();

        cell.Populate(item);
        
        return cell;
        
    }

    public void Reload() {
        if(Table != null)
            Table.tableView.ReloadData();
    }
    
    public void SetItems(List<T> items, bool reload = true) {
        Items = items;
        if(reload)
            Reload();
    }

    public void ScrollToItem(int index, TableView.ScrollPositionType scrollPositionType = TableView.ScrollPositionType.Beginning, bool animated = false) {
        // ReSharper disable once Unity.NoNullPropagation
        Table?.tableView.ScrollToCellWithIdx(index, scrollPositionType, animated);
    }
    
    public virtual void LoadItems() { }

    protected virtual TC CreateCell() {
        
        var type = typeof(TC);
        
        var cell = new GameObject(type.Name, typeof(Touchable)).AddComponent<TC>();
            
        cell.interactable = true;
        cell.reuseIdentifier = type.Name;
        
        using var stream = type.Assembly.GetManifestResourceStream(type.Namespace + "." + type.Name + ".xml");
        using var streamReader = new StreamReader(stream!);
        
        BSMLParser.instance.Parse(streamReader.ReadToEnd(), cell.gameObject, cell);
        
        return cell;

    }
    
}