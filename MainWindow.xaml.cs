﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using JetBrains.Annotations;
using MHW_Editor.Armors;
using MHW_Editor.Assets;
using MHW_Editor.Gems;
using MHW_Editor.Items;
using MHW_Editor.Models;
using MHW_Editor.Skills;
using MHW_Editor.Weapons;
using MHW_Template;
using MHW_Template.Models;
using MHW_Template.Weapons;
using Microsoft.Win32;

namespace MHW_Editor {
    public partial class MainWindow {
#if DEBUG
        private const bool ENABLE_CHEAT_BUTTONS = true;
        private const bool SHOW_RAW_BYTES = true;
#else
        private const bool ENABLE_CHEAT_BUTTONS = false;
        private const bool SHOW_RAW_BYTES = false;
#endif
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        private static readonly string[] FILE_TYPES = {
            "*.am_dat",
            "*.arm_up",
            "*.ask",
            "*.bbtbl",
            "*.dglt",
            "*.diot",
            "*.eq_crt",
            "*.eq_cus",
            "*.itm",
            "*.kire",
            "*.mkex",
            "*.mkit",
            "*.msk",
            "*.new_lb",
            "*.new_lbr",
            "*.owp_dat",
            "*.oam_dat",
            "*.plfe",
            "*.plit",
            "*.rod_inse",
            "*.sgpa",
            "*.shl_tbl",
            "*.skl_dat",
            "*.skl_pt_dat",
            "*.wep_glan",
            "*.wep_saxe",
            "*.wep_wsd",
            "*.wep_wsl",
            "*.wp_dat",
            "*.wp_dat_g"
        };

        private readonly ObservableCollection<dynamic> items = new ObservableCollection<dynamic>();
        private string targetFile;
        private Type targetFileType;
        private Dictionary<string, ColumnHolder> columnMap;
        private bool isManualEditCommit;
        public static Dictionary<ushort, IdNamePair> skillDatLookup = new Dictionary<ushort, IdNamePair>();

        public static string locale = "eng";
        public string Locale {
            get => locale;
            set {
                locale = value;
                foreach (MhwItem item in items) { // Won't warn about duplicate names, but avoid them nonetheless.
                    item.OnPropertyChanged(nameof(IMhwItem.Name),
                                           nameof(SkillDat.Name_And_Id),
                                           nameof(SkillDat.Description),
                                           nameof(EqCrt.Mat_1_Id_button),
                                           nameof(EqCrt.Mat_2_Id_button),
                                           nameof(EqCrt.Mat_3_Id_button),
                                           nameof(EqCrt.Mat_4_Id_button),
                                           nameof(NewLimitBreak.Needed_Item_Id_to_Unlock_button),
                                           nameof(ASkill.Mantle_Item_Id_button),
                                           nameof(Armor.Set_Skill_1_button),
                                           nameof(Armor.Set_Skill_2_button),
                                           nameof(Armor.Skill_1_button),
                                           nameof(Armor.Skill_2_button),
                                           nameof(Armor.Skill_3_button),
                                           nameof(Melee.Skill_button),
                                           nameof(PlantItem.Item_button),
                                           nameof(MelderItem.Result_Item_Id),
                                           nameof(MelderExchange.Source_Item_Id));
                }
            }
        }

        public static bool showIdBeforeName = true;
        public bool ShowIdBeforeName {
            get => showIdBeforeName;
            set {
                showIdBeforeName = value;
                foreach (MhwItem item in items) {
                    item.OnPropertyChanged(nameof(SkillDat.Name_And_Id), nameof(MusicSkill.Song_Id));
                }
            }
        }

        public bool SingleClickToEditMode { get; set; } = true;

        [CanBeNull]
        private CancellationTokenSource savedTimer;

        public MainWindow() {
#pragma warning disable 162
            if (false) {
                // ReSharper disable StringLiteralTypo
                const string chunk = @"V:\MHW\IB\chunk_combined";
                EncryptionHelper.Decrypt(EncryptionKeys.ROD_INSE_KEY, $@"{chunk}\common\equip\rod_insect.rod_inse", $@"{chunk}\common\equip\rod_insect.decrypted.rod_inse");
                Close();
                return;
            }

            InitializeComponent();

            cbx_localization.ItemsSource = Global.LANGUAGE_NAME_LOOKUP;

            dg_items.AutoGeneratingColumn += Dg_items_AutoGeneratingColumn;
            dg_items.AutoGeneratedColumns += Dg_items_AutoGeneratedColumns;
            dg_items.GotFocus += Dg_items_GotFocus;
            dg_items.Sorting += Dg_items_Sorting;
            dg_items.CellEditEnding += Dg_items_CellEditEnding;

            btn_open.Click += Btn_open_Click;
            btn_save.Click += Btn_save_Click;
            btn_customize.Click += Btn_customize_Click;
            btn_slot_cheat.Click += Btn_slot_cheat_Click;
            btn_set_bonus_cheat.Click += Btn_set_bonus_cheat_Click;
            btn_skill_level_cheat.Click += Btn_skill_level_cheat_Click;
            btn_cost_cheat.Click += Btn_cost_cheat_Click;
            btn_damage_cheat.Click += Btn_damage_cheat_Click;
            btn_enable_all_coatings_cheat.Click += Btn_enable_all_coatings_cheat_Click;
            btn_max_sharpness_cheat.Click += Btn_max_sharpness_cheat_Click;
            btn_unlock_skill_limit_cheat.Click += Btn_unlock_skill_limit_cheat_Click;
            btn_sort_jewel_order_by_name.Click += Btn_sort_jewel_order_by_name_Click;

            Width = SystemParameters.MaximizedPrimaryScreenWidth * 0.8;
            Height = SystemParameters.MaximizedPrimaryScreenHeight * 0.5;
#pragma warning restore 162
        }

        private void Dg_items_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e) {
            Debug.Assert(e.PropertyName != null, "e.PropertyName != null");

            switch (e.PropertyName) {
                case nameof(IMhwItem.Bytes):
                case nameof(IMhwItem.Changed):
                case nameof(Melee.GMD_Name_Index):
                case nameof(Melee.GMD_Description_Index):
                    e.Cancel = true; // Internal.
                    break;
                case nameof(IMhwItem.Offset):
                case nameof(IMhwItem.Raw_Data):
                    e.Cancel = !SHOW_RAW_BYTES; // Only for debug builds.
                    break;
                case nameof(Ranged.Barrel_Type):
                case nameof(Ranged.Deviation):
                case nameof(Ranged.Magazine_Type):
                case nameof(Ranged.Muzzle_Type):
                case nameof(Ranged.Scope_Type):
                case nameof(Ranged.Shell_Type_Id):
                    e.Cancel = targetFileType.Is(typeof(Bow));
                    break;
                case nameof(IMhwItem.Name): // None of the following have names.
                    e.Cancel = targetFileType.Is(typeof(ArmUp),
                                                 typeof(ASkill),
                                                 typeof(DecoPercent),
                                                 typeof(EqCrt),
                                                 typeof(EqCus),
                                                 typeof(MelderExchange),
                                                 typeof(MelderItem),
                                                 typeof(MusicSkill),
                                                 typeof(NewLimitBreak),
                                                 typeof(NewLimitBreak2),
                                                 typeof(PlantFertilizer),
                                                 typeof(PlantItem),
                                                 typeof(RodInsect),
                                                 typeof(Sharpness),
                                                 typeof(ShellTable),
                                                 typeof(SkillDat),
                                                 typeof(SkillPointData),
                                                 typeof(WeaponGunLance),
                                                 typeof(WeaponWhistle),
                                                 typeof(WeaponWSword),
                                                 typeof(BottleTable));
                    break;
                case nameof(SkillDat.Id):
                    e.Cancel = targetFileType.Is(typeof(SkillDat));
                    break;
                case nameof(DecoLottery.Item_Id):
                    e.Cancel = targetFileType.Is(typeof(DecoPercent));
                    break;
                case nameof(SkillDat.Index):
                    e.Cancel = targetFileType.Is(typeof(DecoGradeLottery),
                                                 typeof(DecoLottery),
                                                 typeof(Gem),
                                                 typeof(Melee),
                                                 typeof(Ranged),
                                                 typeof(SkillDat));
                    break;
                case nameof(Armor.Set_Skill_1):
                case nameof(Armor.Set_Skill_2):
                case nameof(Armor.Skill_1):
                case nameof(Armor.Skill_2):
                case nameof(Armor.Skill_3):
                case nameof(ASkill.Mantle_Item_Id):
                case nameof(EqCrt.Mat_1_Id):
                case nameof(EqCrt.Mat_2_Id):
                case nameof(EqCrt.Mat_3_Id):
                case nameof(EqCrt.Mat_4_Id):
                case nameof(MelderExchange.Source_Item_Id):
                case nameof(MelderItem.Result_Item_Id):
                case nameof(Melee.Skill):
                case nameof(NewLimitBreak.Needed_Item_Id_to_Unlock):
                case nameof(PlantItem.Item):
                case nameof(SkillDat.Unlock_Skill_1):
                case nameof(SkillDat.Unlock_Skill_2):
                case nameof(SkillDat.Unlock_Skill_3):
                case nameof(SkillDat.Unlock_Skill_4):
                    e.Cancel = true; // Cancel for itemId/skillId columns as we will use a text version with onClick opening a selector.
                    break;
                default:
                    e.Cancel = e.PropertyName.EndsWith("Raw");
                    break;
            }

            if (e.Cancel) return;

            switch (e.PropertyName) {
                case nameof(EqCrt.Item_Category): {
                    var fileName = Path.GetFileNameWithoutExtension(targetFile);
                    if (!EqCrt.categoryLookup.ContainsKey(fileName)) break;

                    var cb = new DataGridComboBoxColumn {
                        Header = e.Column.Header,
                        ItemsSource = EqCrt.categoryLookup[fileName],
                        SelectedValueBinding = new Binding(e.PropertyName),
                        SelectedValuePath = "Key",
                        DisplayMemberPath = "Value",
                        CanUserSort = true
                    };
                    e.Column = cb;
                    break;
                }
            }

            if (e.PropertyName.EndsWith("_percent")) {
                var cb = new DataGridTextColumn {
                    Header = e.Column.Header,
                    Binding = new Binding(e.PropertyName) {
                        StringFormat = "{0:0.##%;-0.##%;\"\"}" // Can't be negative, but needed to hide all 0 cases.
                    },
                    CanUserSort = true,
                    IsReadOnly = true
                };
                e.Column = cb;
            }

            e.Column.CanUserSort = true;

            // Use [DisplayName] attribute for the column header text.
            // Use [SortOrder] attribute to control the position. Generated fields have spacing so it's easy to say 'generated_field_sortOrder + 1'.
            // Use [CustomSorter] to define an IComparer class to control sorting.
            Type sourceClassType = ((dynamic) e.PropertyDescriptor).ComponentType;
            var propertyInfo = sourceClassType.GetProperties().FirstOrDefault(info => info.Name == e.PropertyName);

            var displayName = ((DisplayNameAttribute) propertyInfo?.GetCustomAttribute(typeof(DisplayNameAttribute), true))?.DisplayName;
            var sortOrder = ((SortOrderAttribute) propertyInfo?.GetCustomAttribute(typeof(SortOrderAttribute), true))?.sortOrder;
            var customSorterType = ((CustomSorterAttribute) propertyInfo?.GetCustomAttribute(typeof(CustomSorterAttribute), true))?.customSorterType;
            ICustomSorter customSorter = null;

            if (displayName != null) {
                e.Column.Header = displayName;
            }

            if (customSorterType != null) {
                customSorter = (ICustomSorter) Activator.CreateInstance(customSorterType);
                if (customSorter is ICustomSorterWithPropertyName csWithName) {
                    csWithName.PropertyName = e.PropertyName;
                }
            }

            columnMap[e.PropertyName] = new ColumnHolder(e.Column, sortOrder ?? -1, customSorter);

            // TODO: Fix enum value display at some point.
        }

        private void Dg_items_AutoGeneratedColumns(object sender, EventArgs e) {
            var columns = columnMap.Values.ToList();
            columns.Sort((c1, c2) => c1.sortOrder.CompareTo(c2.sortOrder));
            for (var i = 0; i < columns.Count; i++) {
                columns[i].column.DisplayIndex = i;
            }
        }

        private void Dg_items_GotFocus(object sender, RoutedEventArgs e) {
            // Lookup for the source to be DataGridCell
            if (SingleClickToEditMode && e.OriginalSource is DataGridCell cell) {
                // Needs to only happen when it's a button. If not, we stop regular fields from working.
                if (CheckCellForButtonTypeAndHandleClick(cell)) return;

                // Starts the Edit on the row;
                dg_items.BeginEdit(e);

                if (cell.Content is ComboBox cbx) {
                    cbx.IsDropDownOpen = true;
                }
            }
        }

        private void Dg_items_cell_MouseClick(object sender, MouseButtonEventArgs e) {
            if (sender is DataGridCell cell) {
                // We come here on both single & double click. If we don't check for focus, this hijacks the click and prevents focusing.
                if (e?.ClickCount == 1 && !cell.IsFocused) return;

                CheckCellForButtonTypeAndHandleClick(cell);
            }
        }

        private bool CheckCellForButtonTypeAndHandleClick(DataGridCell cell) {
            if (!(cell.Content is TextBlock)) return false;

            // Have to loop though our column list to file the original property name.
            foreach (var propertyName in columnMap.Keys.Where(key => key.Contains("_button"))) {
                if (cell.Column != columnMap[propertyName].column) continue;

                EditSelectedItemId(cell, propertyName);
                return true;
            }

            return false;
        }

        private void EditSelectedItemId(FrameworkElement cell, string propertyName) {
            var obj = (MhwItem) cell.DataContext;
            var property = obj.GetType().GetProperty(propertyName.Replace("_button", ""), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            Debug.Assert(property != null, nameof(property) + " != null");

            var value = (ushort) Convert.ChangeType(property.GetValue(obj), TypeCode.UInt16);
            var dataSourceType = ((DataSourceAttribute) property.GetCustomAttribute(typeof(DataSourceAttribute), true))?.dataType;

            Dictionary<ushort, IdNamePair> dataSource;
            switch (dataSourceType) {
                case DataSourceType.Items:
                    dataSource = DataHelper.itemData[locale];
                    break;
                case DataSourceType.Skills:
                    dataSource = DataHelper.skillData[locale];
                    break;
                case DataSourceType.SkillDat:
                    dataSource = skillDatLookup;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            var getNewItemId = new GetNewItemId(value, dataSource);
            getNewItemId.ShowDialog();

            if (!getNewItemId.cancelled) {
                property.SetValue(obj, getNewItemId.currentItem);
                obj.OnPropertyChanged(propertyName);
            }
        }

        private void Dg_items_Sorting(object sender, DataGridSortingEventArgs e) {
            // Does the column we're sorting define a custom sorter?
            var matches = columnMap.Where(pair => pair.Value.column == e.Column && pair.Value.customSorter != null).ToList();
            if (!matches.Any()) return;
            var customSorter = matches.First().Value.customSorter;

            e.Column.SortDirection = customSorter.SortDirection = (e.Column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;

            var listColView = (ListCollectionView) dg_items.ItemsSource;
            listColView.CustomSort = customSorter;

            e.Handled = true;
        }

        private void Dg_items_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            // Commit as cell edit ends instead of DG waiting till we leave the row.
            if (!isManualEditCommit) {
                isManualEditCommit = true;
                dg_items.CommitEdit(DataGridEditingUnit.Row, true);
                isManualEditCommit = false;
            }

            CalculatePercents();
        }

        private void CalculatePercents() {
            if (string.IsNullOrEmpty(targetFile)) return;

            if (!targetFileType.Is(typeof(DecoPercent))) return;

            var dict = new Dictionary<int, uint>();
            for (var i = 5; i <= 13; i++) {
                dict[i] = 0;
            }

            foreach (var item in items) {
                dict[5] += item.R5;
                dict[6] += item.R6;
                dict[7] += item.R7;
                dict[8] += item.R8;
                dict[9] += item.R9;
                dict[10] += item.R10;
                dict[11] += item.R11;
                dict[12] += item.R12;
                dict[13] += item.R13;
            }

            foreach (var item in items) {
                item.R5_percent = item.R5 > 0f ? (float) item.R5 / dict[5] : 0f;
                item.R6_percent = item.R6 > 0f ? (float) item.R6 / dict[6] : 0f;
                item.R7_percent = item.R7 > 0f ? (float) item.R7 / dict[7] : 0f;
                item.R8_percent = item.R8 > 0f ? (float) item.R8 / dict[8] : 0f;
                item.R9_percent = item.R9 > 0f ? (float) item.R9 / dict[9] : 0f;
                item.R10_percent = item.R10 > 0f ? (float) item.R10 / dict[10] : 0f;
                item.R11_percent = item.R11 > 0f ? (float) item.R11 / dict[11] : 0f;
                item.R12_percent = item.R12 > 0f ? (float) item.R12 / dict[12] : 0f;
                item.R13_percent = item.R13 > 0f ? (float) item.R13 / dict[13] : 0f;
            }
        }

        private void FillSkillDatDictionary() {
            // Makes the lookup table for skill dat unlock columns which reference themselves by index.
            skillDatLookup = new Dictionary<ushort, IdNamePair>();
            foreach (SkillDat item in items) {
                skillDatLookup[(ushort) item.Index] = new IdNamePair((ushort) item.Index, item.Name_And_Id.name);
            }
        }

        private void Btn_open_Click(object sender, RoutedEventArgs e) {
            var target = Open();
            if (string.IsNullOrEmpty(target)) return;
            Load(target);

            if (targetFileType.Is(typeof(SkillDat))) {
                FillSkillDatDictionary();
            }

            columnMap = new Dictionary<string, ColumnHolder>();
            dg_items.ItemsSource = null;
            dg_items.ItemsSource = new ListCollectionView(items);

            if (targetFileType.Is(typeof(DecoPercent))) {
                CalculatePercents();
            }
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(targetFile)) return;
            Save();
        }

        private void Btn_customize_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(targetFile)) return;

            if (!targetFileType.Is(typeof(SkillDat), typeof(Armor))) return;

            foreach (var item in items) {
                switch (item) {
                    case SkillDat _: {
                        SkillDat skillDat = item;

                        switch (skillDat.Id) {
                            case SkillDataValueClass.Scholar:
                            case SkillDataValueClass.Scenthound:
                                skillDat.Param_5 = 5000;
                                break;
                            case SkillDataValueClass.Tool_Specialist:
                                skillDat.Param_5 = 1;
                                break;
                            case SkillDataValueClass.Item_Prolonger:
                                skillDat.Param_5 = 5000;
                                break;
                            case SkillDataValueClass.Focus:
                                skillDat.Param_6 = 1;
                                break;
                        }

                        break;
                    }
                    case Armor _: {
                        Armor armor = item;

                        switch (armor.Id) {
                            case ArmorDataValueClass.Guildwork_Head_a_plus:
                                armor.Skill_1 = SkillDataValueClass.Effluvial_Expert;
                                armor.Skill_1_Level = 10;
                                armor.Skill_2 = SkillDataValueClass.Scholar;
                                armor.Skill_2_Level = 10;
                                armor.Skill_3 = SkillDataValueClass.Aquatic_Polar_Mobility;
                                armor.Skill_3_Level = 10;
                                break;
                            case ArmorDataValueClass.Kushala_Cista_a_plus:
                                armor.Skill_1 = SkillDataValueClass.Bow_Charge_Plus;
                                armor.Skill_1_Level = 10;
                                armor.Skill_2 = SkillDataValueClass.Coldproof;
                                armor.Skill_2_Level = 10;
                                armor.Skill_3 = SkillDataValueClass.Defense_Boost;
                                armor.Skill_3_Level = 10;
                                break;
                            case ArmorDataValueClass.Kirin_Longarms_a_plus:
                                armor.Skill_1 = SkillDataValueClass.Effluvia_Resistance;
                                armor.Skill_1_Level = 10;
                                armor.Skill_2 = SkillDataValueClass.Focus;
                                armor.Skill_2_Level = 10;
                                armor.Skill_3 = SkillDataValueClass.Free_Meal;
                                armor.Skill_3_Level = 10;
                                break;
                            case ArmorDataValueClass.Fellshroud_Coil_a_plus:
                                armor.Skill_1 = SkillDataValueClass.Guard;
                                armor.Skill_1_Level = 10;
                                armor.Skill_2 = SkillDataValueClass.Guard_Up;
                                armor.Skill_2_Level = 10;
                                armor.Skill_3 = SkillDataValueClass.Heat_Guard;
                                armor.Skill_3_Level = 10;
                                break;
                            case ArmorDataValueClass.Rimeguard_Greaves_a_plus:
                                armor.Skill_1 = SkillDataValueClass.Hunger_Resistance;
                                armor.Skill_1_Level = 10;
                                armor.Skill_2 = SkillDataValueClass.Maximum_Might;
                                armor.Skill_2_Level = 10;
                                armor.Skill_3 = SkillDataValueClass.Mind_s_Eye_Ballistics;
                                armor.Skill_3_Level = 10;
                                break;
                            case ArmorDataValueClass.Critical_Charm_II:
                                armor.Skill_1 = SkillDataValueClass.Non_elemental_Boost;
                                armor.Skill_1_Level = 10;
                                armor.Skill_2 = SkillDataValueClass.Power_Prolonger;
                                armor.Skill_2_Level = 10;
                                armor.Skill_3 = SkillDataValueClass.Quick_Sheath;
                                armor.Skill_3_Level = 10;
                                break;
                            // Gathering Set
                            case ArmorDataValueClass.Guildwork_Body_a_plus:
                                armor.Skill_1 = SkillDataValueClass.Cliffhanger;
                                armor.Skill_1_Level = 10;
                                armor.Skill_2 = SkillDataValueClass.Quick_Sheath;
                                armor.Skill_2_Level = 10;
                                armor.Skill_3 = SkillDataValueClass.Recovery_Speed;
                                armor.Skill_3_Level = 10;
                                break;
                            case ArmorDataValueClass.Guildwork_Braces_a_plus:
                                armor.Skill_1 = SkillDataValueClass.Honey_Hunter;
                                armor.Skill_1_Level = 10;
                                armor.Skill_2 = SkillDataValueClass.Master_Gatherer;
                                armor.Skill_2_Level = 10;
                                armor.Skill_3 = SkillDataValueClass.Pro_Transporter;
                                armor.Skill_3_Level = 10;
                                break;
                            case ArmorDataValueClass.Guildwork_Waist_a_plus:
                                armor.Skill_1 = SkillDataValueClass.Scoutfly_Range_Up;
                                armor.Skill_1_Level = 10;
                                armor.Skill_2 = SkillDataValueClass.Coldproof;
                                armor.Skill_2_Level = 10;
                                armor.Skill_3 = SkillDataValueClass.Heat_Guard;
                                armor.Skill_3_Level = 10;
                                break;
                            case ArmorDataValueClass.Guildwork_Feet_a_plus:
                                armor.Skill_1 = SkillDataValueClass.Hunger_Resistance;
                                armor.Skill_1_Level = 10;
                                armor.Skill_2 = SkillDataValueClass.Item_Prolonger;
                                armor.Skill_2_Level = 10;
                                armor.Skill_3 = SkillDataValueClass.Paralysis_Resistance;
                                armor.Skill_3_Level = 10;
                                break;
                            case ArmorDataValueClass.Tremor_Charm_III:
                                armor.Skill_1 = SkillDataValueClass.Forager_s_Luck;
                                armor.Skill_1_Level = 10;
                                armor.Skill_2 = SkillDataValueClass.Detector;
                                armor.Skill_2_Level = 10;
                                armor.Skill_3 = SkillDataValueClass.Scenthound;
                                armor.Skill_3_Level = 10;
                                break;
                        }

                        break;
                    }
                }
            }
        }

        private void Btn_slot_cheat_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(targetFile)) return;

            if (!targetFileType.Is(typeof(IWeapon), typeof(Armor), typeof(ASkill))) return;

            foreach (var item in items) {
                switch (item) {
                    case ISlots _: {
                        item.Slot_Count = 3;
                        item.Slot_1_Size = 4;
                        item.Slot_2_Size = 4;
                        item.Slot_3_Size = 4;

                        break;
                    }
                    case ASkill _: {
                        ASkill aSkill = item;
                        aSkill.Deco_Count = 2;
                        aSkill.Deco_Lvl_1 = 4;
                        aSkill.Deco_Lvl_2 = 4;

                        break;
                    }
                }
            }
        }

        private void Btn_set_bonus_cheat_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(targetFile)) return;

            if (!targetFileType.Is(typeof(Armor))) return;

            foreach (Armor item in items) {
                if (item.Set_Skill_1_Level > 0) item.Set_Skill_1_Level = 5;
            }
        }

        private void Btn_skill_level_cheat_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(targetFile)) return;

            if (!targetFileType.Is(typeof(Gem), typeof(Armor))) return;

            foreach (var item in items) {
                switch (item) {
                    case Gem _: {
                        Gem gem = item;
                        gem.Skill_1_Level = 10;
                        if (gem.Skill_2_Level > 0) gem.Skill_2_Level = 10;
                        break;
                    }
                    case Armor _: {
                        Armor armor = item;
                        if (armor.Skill_1_Level > 0) armor.Skill_1_Level = 10;
                        if (armor.Skill_2_Level > 0) armor.Skill_2_Level = 10;
                        if (armor.Skill_3_Level > 0) armor.Skill_3_Level = 10;
                        break;
                    }
                }
            }
        }

        private void Btn_cost_cheat_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(targetFile)) return;

            if (!targetFileType.Is(typeof(Item), typeof(Armor), typeof(IWeapon), typeof(EqCrt), typeof(EqCus), typeof(NewLimitBreak), typeof(NewLimitBreak2))) return;

            foreach (var item in items) {
                switch (item) {
                    case Item _: {
                        Item itm = item;
                        if (itm.Buy_Price > 0) itm.Buy_Price = 1;
                        break;
                    }
                    case Armor _: {
                        Armor armor = item;
                        if (armor.Cost > 0) armor.Cost = 1;
                        break;
                    }
                    case IWeapon _: {
                        IWeapon weapon = item;
                        if (weapon.Cost > 0) weapon.Cost = 1;
                        break;
                    }
                    case EqCrt _: {
                        EqCrt eqCrt = item;
                        if (eqCrt.Mat_1_Count > 0) eqCrt.Mat_1_Count = 1;
                        if (eqCrt.Mat_2_Count > 0) eqCrt.Mat_2_Count = 1;
                        if (eqCrt.Mat_3_Count > 0) eqCrt.Mat_3_Count = 1;
                        if (eqCrt.Mat_4_Count > 0) eqCrt.Mat_4_Count = 1;
                        break;
                    }
                    case EqCus _: {
                        EqCus eqCus = item;
                        if (eqCus.Mat_1_Count > 0) eqCus.Mat_1_Count = 1;
                        if (eqCus.Mat_2_Count > 0) eqCus.Mat_2_Count = 1;
                        if (eqCus.Mat_3_Count > 0) eqCus.Mat_3_Count = 1;
                        if (eqCus.Mat_4_Count > 0) eqCus.Mat_4_Count = 1;
                        break;
                    }
                    case NewLimitBreak _: {
                        NewLimitBreak newLimitBreak = item;
                        if (newLimitBreak.Mat_1_Count > 0) newLimitBreak.Mat_1_Count = 1;
                        if (newLimitBreak.Mat_2_Count > 0) newLimitBreak.Mat_2_Count = 1;
                        if (newLimitBreak.Mat_3_Count > 0) newLimitBreak.Mat_3_Count = 1;
                        if (newLimitBreak.Mat_4_Count > 0) newLimitBreak.Mat_4_Count = 1;
                        break;
                    }
                    case NewLimitBreak2 _: {
                        NewLimitBreak2 newLimitBreak2 = item;
                        if (newLimitBreak2.Research_Cost_r10_ > 0) newLimitBreak2.Research_Cost_r10_ = 1;
                        if (newLimitBreak2.Research_Cost_r11_ > 0) newLimitBreak2.Research_Cost_r11_ = 1;
                        if (newLimitBreak2.Research_Cost_r12_ > 0) newLimitBreak2.Research_Cost_r12_ = 1;
                        break;
                    }
                }
            }
        }

        private void Btn_damage_cheat_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(targetFile)) return;

            if (!targetFileType.Is(typeof(IWeapon), typeof(OtomoWeaponDat))) return;

            foreach (var item in items) {
                switch (item) {
                    case OtomoWeaponDat _: {
                        OtomoWeaponDat otomoWeapon = item;
                        if (otomoWeapon.Melee_Damage > 0) otomoWeapon.Melee_Damage = 50000;
                        if (otomoWeapon.Ranged_Damage > 0) otomoWeapon.Ranged_Damage = 50000;
                        break;
                    }
                    case IWeapon _: {
                        IWeapon weapon = item;
                        if (weapon.Damage > 0) weapon.Damage = 50000;
                        break;
                    }
                }
            }
        }

        private void Btn_enable_all_coatings_cheat_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(targetFile)) return;

            if (!targetFileType.Is(typeof(BottleTable))) return;

            foreach (BottleTable item in items) {
                item.Close_Range = CoatingType.Plus;
                item.Power = CoatingType.Plus;
                item.Paralysis = CoatingType.Plus;
                item.Poison = CoatingType.Plus;
                item.Sleep = CoatingType.Plus;
                item.Blast = CoatingType.Plus;
            }
        }

        private void Btn_max_sharpness_cheat_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(targetFile)) return;

            if (!targetFileType.Is(typeof(Sharpness), typeof(Melee))) return;

            foreach (var item in items) {
                switch (item) {
                    case Sharpness _: {
                        Sharpness sharpness = item;
                        sharpness.Red = 1;
                        sharpness.Orange = 1;
                        sharpness.Yellow = 1;
                        sharpness.Green = 1;
                        sharpness.Blue = 1;
                        sharpness.White = 1;
                        sharpness.Purple = 400;
                        break;
                    }
                    case Melee _: {
                        Melee weapon = item;
                        if (weapon.Sharpness_Amount > 0) weapon.Sharpness_Amount = 5;
                        break;
                    }
                }
            }
        }

        private void Btn_unlock_skill_limit_cheat_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(targetFile)) return;

            if (!targetFileType.Is(typeof(SkillDat))) return;

            foreach (SkillDat item in items) {
                item.Unlock_Skill_1 = 0;
                item.Unlock_Skill_2 = 0;
                item.Unlock_Skill_3 = 0;
                item.Unlock_Skill_4 = 0;
            }
        }

        private void Btn_sort_jewel_order_by_name_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(targetFile)) return;

            if (!targetFileType.Is(typeof(Item))) return;

            var rawList = new List<GemData>();
            for (var i = 0; i < items.Count; i++) {
                Item item = items[i];
                if (item.Name.Contains(" Jewel")) {
                    rawList.Add(new GemData {index = i, itemName = item.Name, sortOrder = item.Sort_Order});
                }
            }

            // One list of the sorted "sortOrder"s.
            var sortedSortIndexes = new List<GemData>(rawList)
                                    .OrderBy(data => data.sortOrder)
                                    .Select(data => data.sortOrder)
                                    .ToList();
            // And another list of item indexes, sorted by gem name.
            var sortedGemNameIndexes = new List<GemData>(rawList)
                                       .OrderBy(data => data.itemName)
                                       .Select(data => data.index)
                                       .ToList();

            for (var i = 0; i < sortedSortIndexes.Count; i++) {
                var index = sortedGemNameIndexes[i];
                var newSortIndex = sortedSortIndexes[i];
                Item item = items[index];
                item.Sort_Order = newSortIndex;
            }
        }

        private string Open() {
            var ofdResult = new OpenFileDialog {
                Filter = $"MHW Data Files (See mod description for full list.)|{string.Join(";", FILE_TYPES)}",
                Multiselect = false
            };
            ofdResult.ShowDialog();

            return ofdResult.FileName;
        }

        private void Load(string target) {
            targetFile = target;
            targetFileType = GetFileType();
            items.Clear();
            Title = Path.GetFileName(targetFile);

            Debug.Assert(targetFile != null, nameof(targetFile) + " != null");

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
            // ReSharper disable PossibleNullReferenceException
            var initialOffset = (ulong) targetFileType.GetField(nameof(Armor.InitialOffset), flags).GetValue(null);
            var structSize = (uint) targetFileType.GetField(nameof(Armor.StructSize), flags).GetValue(null);
            var entryCountOffset = (long) targetFileType.GetField(nameof(Armor.EntryCountOffset), flags).GetValue(null);
            var encryptionKey = (string) targetFileType.GetField(nameof(Armor.EncryptionKey), flags).GetValue(null);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
#pragma warning disable 162
            if (ENABLE_CHEAT_BUTTONS) {
                btn_customize.Visibility = targetFileType.Is(typeof(Armor), typeof(SkillDat)) ? Visibility.Visible : Visibility.Collapsed;
                btn_slot_cheat.Visibility = targetFileType.Is(typeof(Armor), typeof(IWeapon), typeof(ASkill)) ? Visibility.Visible : Visibility.Collapsed;
                btn_skill_level_cheat.Visibility = targetFileType.Is(typeof(Armor), typeof(Gem)) ? Visibility.Visible : Visibility.Collapsed;
                btn_set_bonus_cheat.Visibility = targetFileType.Is(typeof(Armor)) ? Visibility.Visible : Visibility.Collapsed;
                btn_cost_cheat.Visibility = targetFileType.Is(typeof(Armor), typeof(Item), typeof(IWeapon), typeof(EqCrt), typeof(EqCus), typeof(NewLimitBreak), typeof(NewLimitBreak2)) ? Visibility.Visible : Visibility.Collapsed;
                btn_damage_cheat.Visibility = targetFileType.Is(typeof(IWeapon), typeof(OtomoWeaponDat)) ? Visibility.Visible : Visibility.Collapsed;
                btn_enable_all_coatings_cheat.Visibility = targetFileType.Is(typeof(BottleTable)) ? Visibility.Visible : Visibility.Collapsed;
                btn_max_sharpness_cheat.Visibility = targetFileType.Is(typeof(Sharpness), typeof(Melee)) ? Visibility.Visible : Visibility.Collapsed;
                btn_unlock_skill_limit_cheat.Visibility = targetFileType.Is(typeof(SkillDat)) ? Visibility.Visible : Visibility.Collapsed;
            }
#pragma warning restore 162

            btn_sort_jewel_order_by_name.Visibility = targetFileType.Is(typeof(Item)) ? Visibility.Visible : Visibility.Collapsed;
            cb_show_id_before_name.Visibility = targetFileType.Is(typeof(SkillDat), typeof(DecoPercent), typeof(MusicSkill)) ? Visibility.Visible : Visibility.Collapsed;

            var weaponFilename = Path.GetFileNameWithoutExtension(targetFile);

            try {
                if (encryptionKey != null) {
                    // Read & decrypt file.
                    var encryptedBytes = File.ReadAllBytes(targetFile);
                    var decryptedBytes = EncryptionHelper.Decrypt(encryptionKey, encryptedBytes);

                    using (var dat = new MemoryStream()) {
                        // Write to a stream for the loader. Leave base stream OPEN.
                        using (var writer = new BinaryWriter(dat, Encoding.Default, true)) {
                            writer.Write(decryptedBytes);
                        }

                        // Load as normal.
                        using (var reader = new BinaryReader(dat)) {
                            ReadStructs(reader, structSize, initialOffset, weaponFilename, entryCountOffset);
                        }
                    }
                } else { // No encryption, just open, read, & close.
                    using (var dat = new BinaryReader(new FileStream(targetFile, FileMode.Open, FileAccess.Read))) {
                        ReadStructs(dat, structSize, initialOffset, weaponFilename, entryCountOffset);
                    }
                }
            } catch (Exception e) {
                MessageBox.Show(this, e.Message, "Load Error");
            }
        }

        private void ReadStructs(BinaryReader dat, uint structSize, ulong initialOffset, string weaponFilename, long entryCountOffset) {
            if (entryCountOffset >= 0) {
                ReadStructsAsKnownLength(dat, structSize, initialOffset, weaponFilename, entryCountOffset);
            } else {
                ReadStructsAsUnknownLength(dat, structSize, initialOffset, weaponFilename);
            }
        }

        private void ReadStructsAsKnownLength(BinaryReader dat, uint structSize, ulong initialOffset, string weaponFilename, long entryCountOffset) {
            dat.BaseStream.Seek(entryCountOffset, SeekOrigin.Begin);
            var count = dat.ReadUInt32();

            dat.BaseStream.Seek((long) initialOffset, SeekOrigin.Begin);

            for (var i = 0; i < count; i++) {
                var position = dat.BaseStream.Position;
                var buff = dat.ReadBytes((int) structSize);

                object obj;
                if (targetFileType.Is(typeof(IWeapon))) {
                    obj = Activator.CreateInstance(targetFileType, buff, (ulong) position, weaponFilename);
                } else {
                    obj = Activator.CreateInstance(targetFileType, buff, (ulong) position);
                }

                if (obj == null) return;

                items.Add(obj);
            }
        }

        private void ReadStructsAsUnknownLength(BinaryReader dat, uint structSize, ulong offset, string weaponFilename) {
            var len = (ulong) dat.BaseStream.Length;
            do {
                var buff = new byte[structSize];
                dat.BaseStream.Seek((long) offset, SeekOrigin.Begin);
                dat.Read(buff, 0, (int) structSize);

                object obj;
                if (targetFileType.Is(typeof(IWeapon))) {
                    obj = Activator.CreateInstance(targetFileType, buff, offset, weaponFilename);
                } else {
                    obj = Activator.CreateInstance(targetFileType, buff, offset);
                }

                if (obj == null) return;

                items.Add(obj);

                offset += structSize;
            } while (offset + structSize <= len);
        }

        private async void Save() {
            try {
                bool changesSaved;
                var encryptionKey = (string) targetFileType.GetField(nameof(Armor.EncryptionKey), BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).GetValue(null);

                if (encryptionKey != null) {
                    // Read & decrypt file.
                    var encryptedBytes = File.ReadAllBytes(targetFile);
                    var decryptedBytes = EncryptionHelper.Decrypt(encryptionKey, encryptedBytes);

                    using (var dat = new MemoryStream()) {
                        // Write to a stream for the loader. Leave base stream OPEN.
                        using (var writer = new BinaryWriter(dat, Encoding.Default, true)) {
                            writer.Write(decryptedBytes);
                        }

                        // Save as normal. Leave base stream OPEN.
                        using (var writer = new BinaryWriter(dat, Encoding.Default, true)) {
                            changesSaved = WriteChanges(writer);
                        }

                        // If there are no changes, then we don't need to write the result back out.
                        if (changesSaved) {
                            // Re-encrypt and write it back out.
                            decryptedBytes = dat.ToArray();
                            encryptedBytes = EncryptionHelper.Encrypt(encryptionKey, decryptedBytes);
                            File.WriteAllBytes(targetFile, encryptedBytes);
                        }
                    }
                } else { // No encryption, just open, write, & close.
                    using (var dat = new BinaryWriter(new FileStream(targetFile, FileMode.Open, FileAccess.Write))) {
                        changesSaved = WriteChanges(dat);
                    }
                }

                savedTimer?.Cancel();
                savedTimer = new CancellationTokenSource();
                lbl_saved.Visibility = changesSaved ? Visibility.Visible : Visibility.Collapsed;
                lbl_no_changes.Visibility = changesSaved ? Visibility.Collapsed : Visibility.Visible;
                try {
                    await Task.Delay(3000, savedTimer.Token);
                    lbl_saved.Visibility = lbl_no_changes.Visibility = Visibility.Hidden;
                } catch (TaskCanceledException) {
                }
            } catch (Exception e) {
                MessageBox.Show(this, e.Message, "Save Error");
            }
        }

        private bool WriteChanges(BinaryWriter dat) {
            var changesSaved = false;

            foreach (IMhwItem item in items) {
                if (item.Offset == 0 || !item.Changed) continue;

                dat.BaseStream.Seek((long) item.Offset, SeekOrigin.Begin);
                dat.Write(item.Bytes);

                item.Changed = false;
                changesSaved = true;
            }

            return changesSaved;
        }

        private Type GetFileType() {
            var fileName = Path.GetFileName(targetFile);
            Debug.Assert(fileName != null, nameof(fileName) + " != null");

            if (fileName.EndsWith(".am_dat")) return typeof(Armor);
            if (fileName.EndsWith(".arm_up")) return typeof(ArmUp);
            if (fileName.EndsWith(".ask")) return typeof(ASkill);
            if (fileName.EndsWith(".bbtbl")) return typeof(BottleTable);
            if (fileName.EndsWith(".dglt")) return typeof(DecoGradeLottery);
            if (fileName.EndsWith(".diot")) return typeof(DecoLottery);
            if (fileName.EndsWith(".eq_crt")) return typeof(EqCrt);
            if (fileName.EndsWith(".eq_cus")) return typeof(EqCus);
            if (fileName.EndsWith(".itm")) return typeof(Item);
            if (fileName.EndsWith(".kire")) return typeof(Sharpness);
            if (fileName.EndsWith(".mkex")) return typeof(MelderExchange);
            if (fileName.EndsWith(".mkit")) return typeof(MelderItem);
            if (fileName.EndsWith(".msk")) return typeof(MusicSkill);
            if (fileName.EndsWith(".new_lb")) return typeof(NewLimitBreak2);
            if (fileName.EndsWith(".new_lbr")) return typeof(NewLimitBreak);
            if (fileName.EndsWith(".oam_dat")) return typeof(OtomoArmorDat);
            if (fileName.EndsWith(".owp_dat")) return typeof(OtomoWeaponDat);
            if (fileName.EndsWith(".plfe")) return typeof(PlantFertilizer);
            if (fileName.EndsWith(".plit")) return typeof(PlantItem);
            if (fileName.EndsWith(".rod_inse")) return typeof(RodInsect);
            if (fileName.EndsWith(".sgpa")) return typeof(Gem);
            if (fileName.EndsWith(".shl_tbl")) return typeof(ShellTable);
            if (fileName.EndsWith(".skl_dat")) return typeof(SkillDat);
            if (fileName.EndsWith(".skl_pt_dat")) return typeof(SkillPointData);
            if (fileName.EndsWith(".wep_glan")) return typeof(WeaponGunLance);
            if (fileName.EndsWith(".wep_saxe")) return typeof(WeaponSwitchAxe);
            if (fileName.EndsWith(".wep_wsd")) return typeof(WeaponWSword);
            if (fileName.EndsWith(".wep_wsl")) return typeof(WeaponWhistle);
            if (fileName.EndsWith(".wp_dat")) return typeof(Melee);
            if (fileName.EndsWith(".wp_dat_g")) {
                if (fileName.StartsWith("bow")) return typeof(Bow);
                if (fileName.StartsWith("lbg") || fileName.StartsWith("hbg")) return typeof(BowGun);
            }

            throw new Exception($"No type found for: {fileName}");
        }
    }
}