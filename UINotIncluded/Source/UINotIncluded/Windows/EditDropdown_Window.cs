using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UINotIncluded.Widget;
using UnityEngine;
using Verse;

namespace UINotIncluded.Windows
{
    public class EditDropdown_Window : EditMainButton_Window
    {
        private List<Widget.Configs.ElementConfig> cacheAvaibleElements;
        public EditDropdown_Window(Widget.Configs.DropdownMenuConfig config) : base(config) { }

        public override Vector2 InitialSize => new Vector2(786f, 600f);

        public override void DoWindowContents(Rect rect)
        {
            Rect firstHalf = new Rect(rect.x, rect.y, 314f, rect.height);
            Rect secondHalf = new Rect(firstHalf.xMax + 36, rect.y, 400f, rect.height);

            secondHalf.height -= Window.CloseButSize.y;
            Verse.Text.Font = GameFont.Medium;
            Widgets.Label(new Rect(secondHalf.x, secondHalf.y, secondHalf.width, 35f), "Edit Dropdown");
            Verse.Text.Font = GameFont.Small;
            secondHalf.yMin += 45f;
            Listing_Standard list = new Listing_Standard();
            list.Begin(secondHalf);
            list.Label(string.Format("Width ({0}px)", Math.Round(((Widget.Configs.DropdownMenuConfig)config).width).ToString()));
            ((Widget.Configs.DropdownMenuConfig)config).width = list.Slider(((Widget.Configs.DropdownMenuConfig)config).width, 50f, 400f);

            list.Label(string.Format("Spacing ({0}px)", Math.Round(((Widget.Configs.DropdownMenuConfig)config).spacing).ToString()));
            ((Widget.Configs.DropdownMenuConfig)config).spacing = list.Slider(((Widget.Configs.DropdownMenuConfig)config).spacing, 0f, 10f);
            list.End();

            float optionsHeight = list.MaxColumnHeightSeen;
            Rect dragableRect = new Rect(secondHalf.x, secondHalf.y + optionsHeight, secondHalf.width, secondHalf.height - optionsHeight);
            DoElementDraggables(dragableRect);

            base.DoWindowContents(firstHalf);
        }

        private void DoElementDraggables(Rect rect)
        {
            float columnWidth = rect.width / 2;
            float curY = rect.y;

            if (cacheAvaibleElements == null) cacheAvaibleElements = WidgetManager.AvailableSelectedWidgets(true).Where(e => e.GetType() != typeof(Widget.Configs.DropdownMenuConfig)).ToList();

            DragManager<Widget.Configs.ElementConfig> manager = new DragManager<Widget.Configs.ElementConfig>(
                OnUpdate: () => cacheAvaibleElements = null,
                GetLabel: (Widget.Configs.ElementConfig element) => { return element.SettingLabel; },
                OnClick: (Widget.Configs.ElementConfig element) => {
                    if (element.Configurable) return element.Worker.OpenConfigWindow;
                    return null;
                });

            Rect selectTypeRect = new Rect(rect.x, curY, columnWidth, 30f);
            if (Widgets.ButtonText(selectTypeRect.ContractedBy(2f), WidgetManager.SelectedGetterName))
            {
                List<FloatMenuOption> options = new List<FloatMenuOption>();
                foreach (String widgetGetter in WidgetManager.availableGetters.Keys)
                {
                    options.Add(new FloatMenuOption(widgetGetter, (Action)(() => { WidgetManager.SelectGetter(widgetGetter); cacheAvaibleElements = null; })));
                }
                Find.WindowStack.Add((Window)new FloatMenu(options));
            }

            DragMemory.hoveringOver = null;

            Widget.CustomLists.Draggable<Widget.Configs.ElementConfig>("AvailableDrop", new Rect(rect.x, curY + selectTypeRect.height, columnWidth, rect.height - selectTypeRect.height).ContractedBy(3f), cacheAvaibleElements, (Widget.Configs.ElementConfig element) => { return element.SettingLabel; }, manager, false);
            Widget.CustomLists.Draggable<Widget.Configs.ElementConfig>("Dropdown", new Rect(rect.x + columnWidth, curY, columnWidth, rect.height).ContractedBy(3f), ((Widget.Configs.DropdownMenuConfig)config).elements, (Widget.Configs.ElementConfig element) => { return element.SettingLabel; }, manager);

            manager.Update();
        }
    }
}
