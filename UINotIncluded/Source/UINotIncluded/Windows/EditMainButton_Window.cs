using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace UINotIncluded.Windows
{
    class EditMainButton_Window : Window
    {
        private readonly MainButtonMemory buttonMemory;
        //private string newLabel;
        //private string iconPath;
        //private bool newMinimized;
        private Vector2 scrollPos;
        private float viewHeight;
        private const int IconSize = 40;
        private const int IconPadding = 5;
        private const int IconMargin = 5;
        private const int ColorSize = 22;
        private const int ColorPadding = 2;
        private static readonly Vector2 ButSize = new Vector2(150f, 38f);
        private static readonly float EditFieldHeight = 30f;
        private static readonly float ResetButtonWidth = 60f;
        private static readonly Regex ValidSymbolRegex = new Regex("^[\\p{L}0-9 '\\-]*$");
        private const int MaxSymbolLength = 40;
        static private readonly Dictionary<string, Texture2D> cacheIcons = new Dictionary<string, Texture2D>();
        static private List<string> defaultIconPathList;

        public override Vector2 InitialSize => new Vector2(350f, 500f);

        public EditMainButton_Window(MainButtonMemory buttonMemory)
        {
            this.buttonMemory = buttonMemory;
            this.absorbInputAroundWindow = true;
        }

        public override void OnAcceptKeyPressed()
        {
            this.TryAccept();
            Event.current.Use();
        }

        public override void DoWindowContents(Rect rect)
        {
            Rect inRect = rect;
            inRect.height -= Window.CloseButSize.y;
            Verse.Text.Font = GameFont.Medium;
            Widgets.Label(new Rect(inRect.x, inRect.y, rect.width, 35f), "Edit Buttons".Translate());
            Verse.Text.Font = GameFont.Small;            
            string defaultLabel = (string)"Default".Translate();
            inRect.yMin += 45f;
            float curY = inRect.y;
            float iconX = (float)Math.Floor(inRect.width / 2) - 44f;
            GUI.DrawTexture(new Rect(iconX, curY + 5f, 88f, 88f), (Texture)this.buttonMemory.Def.Icon);
            curY += 93f;
            float x = inRect.x + inRect.width / 3f;
            float width = (float)((double)inRect.xMax - (double)x - (double)EditMainButton_Window.ResetButtonWidth - 10.0);
            float labelY = curY;
            Widgets.Label(inRect.x, ref labelY, inRect.width, (string)"Name".Translate());
            this.buttonMemory.label = Widgets.TextField(new Rect(x, curY, width, EditMainButton_Window.EditFieldHeight), this.buttonMemory.label, 40, EditMainButton_Window.ValidSymbolRegex);


            Rect labelRect = new Rect(x, curY, width, EditMainButton_Window.EditFieldHeight);
            Rect defaultLabelRect = new Rect(labelRect.xMax + 10f, curY, EditMainButton_Window.ResetButtonWidth, EditMainButton_Window.EditFieldHeight);

            if (Widgets.ButtonText(defaultLabelRect, defaultLabel))
            {
                SoundDefOf.Click.PlayOneShotOnCamera();
                this.buttonMemory.label = buttonMemory.defaultLabel;
            }
            curY += (EditMainButton_Window.EditFieldHeight + 10f);


            Rect minimizedRect = new Rect(inRect.x, curY, width *2 - Widgets.CheckboxSize - 10f, EditMainButton_Window.EditFieldHeight);
            Rect defaultMinimizedRect = new Rect(minimizedRect.xMax + 10f, curY, EditMainButton_Window.ResetButtonWidth, EditMainButton_Window.EditFieldHeight);
            Widgets.CheckboxLabeled(minimizedRect,"Minimized".Translate(),ref buttonMemory.minimized);

            if (Widgets.ButtonText(defaultMinimizedRect, defaultLabel))
            {
                SoundDefOf.Click.PlayOneShotOnCamera();
                this.buttonMemory.minimized = this.buttonMemory.defaultMinimized;
            }


            curY += (EditMainButton_Window.EditFieldHeight + 10f);
            Rect mainRect = inRect;
            mainRect.yMax -= 4f;
            Widgets.Label(mainRect.x, ref curY, mainRect.width, (string)"Icon".Translate());
            mainRect.yMin = curY;
            this.DoIconSelector(mainRect);
            buttonMemory.Update();
            if (Widgets.ButtonText(new Rect(0.0f, rect.height - EditMainButton_Window.ButSize.y, EditMainButton_Window.ButSize.x, EditMainButton_Window.ButSize.y), (string)"Reset".Translate()))
            {
                buttonMemory.Reset();
                this.Close();
            }
            if (!Widgets.ButtonText(new Rect(rect.width - EditMainButton_Window.ButSize.x, rect.height - EditMainButton_Window.ButSize.y, EditMainButton_Window.ButSize.x, EditMainButton_Window.ButSize.y), (string)"DoneButton".Translate()))
                return;
            this.TryAccept();
        }

        private void DoIconSelector(Rect mainRect)
        {
            int num1 = 50;
            Rect viewRect = new Rect(0.0f, 0.0f, mainRect.width - 16f, this.viewHeight);
            Widgets.BeginScrollView(mainRect, ref this.scrollPos, viewRect);
            IEnumerable<string> allPaths = GetAvaibleIcons();
            int num2 = Mathf.FloorToInt(viewRect.width / (float)(num1 + 5));
            int num3 = allPaths.Count();
            int num4 = 0;
            foreach (string iconPath in allPaths)
            {
                if (iconPath != null && !cacheIcons.ContainsKey(iconPath)) cacheIcons.Add(iconPath, ContentFinder<Texture2D>.Get(iconPath));
                int num5 = num4 / num2;
                int num6 = num4 % num2;
                int num7 = num4 >= num3 - num3 % num2 ? num3 % num2 : num2;
                Rect rect = new Rect((float)(((double)viewRect.width - (double)(num7 * num1) - (double)((num7 - 1) * 5)) / 2.0) + (float)(num6 * num1) + (float)(num6 * 5), (float)(num5 * num1 + num5 * 5), (float)num1, (float)num1);
                Widgets.DrawLightHighlight(rect);
                Widgets.DrawHighlightIfMouseover(rect);
                if (iconPath == this.buttonMemory.iconPath)
                    Widgets.DrawBox(rect);
                if (iconPath != null) GUI.DrawTexture(new Rect(rect.x + 5f, rect.y + 5f, 40f, 40f), cacheIcons[iconPath]);
                GUI.color = Color.white;
                if (Widgets.ButtonInvisible(rect))
                {
                    this.buttonMemory.iconPath = iconPath;
                    SoundDefOf.Tick_High.PlayOneShotOnCamera();
                }
                this.viewHeight = Mathf.Max(this.viewHeight, rect.yMax);
                ++num4;
            }
            GUI.color = Color.white;
            Widgets.EndScrollView();
        }

        private IEnumerable<string> GetAvaibleIcons()
        {
            if (defaultIconPathList == null)
            {
                defaultIconPathList = new List<string>();
                foreach (MainButtonDef button in DefDatabase<MainButtonDef>.AllDefs)
                {
                    if(button.iconPath != null) defaultIconPathList.Add(button.iconPath);
                }
            }
            yield return null;
            foreach (string path in defaultIconPathList) yield return path;
        }

        //private void DoColorSelector(Rect mainRect, ref float curY)
        //{
        //    int num1 = 26;
        //    float num2 = 98f;
        //    int num3 = Mathf.FloorToInt((mainRect.width - num2) / (float)(num1 + 2));
        //    int num4 = Mathf.CeilToInt((float)EditMainButton_Window.IdeoColorsSorted.Count / (float)num3);
        //    GUI.BeginGroup(mainRect);
        //    GUI.color = this.newColorDef.color;
        //    GUI.DrawTexture(new Rect(5f, 5f, 88f, 88f), (Texture)this.newIconDef.Icon);
        //    GUI.color = Color.white;
        //    curY += num2;
        //    int num5 = 0;
        //    foreach (ColorDef colorDef in EditMainButton_Window.IdeoColorsSorted)
        //    {
        //        int num6 = num5 / num3;
        //        int num7 = num5 % num3;
        //        float num8 = (float)(((double)num2 - (double)(num1 * num4) - 2.0) / 2.0);
        //        Rect rect = new Rect(num2 + (float)(num7 * num1) + (float)(num7 * 2), num8 + (float)(num6 * num1) + (float)(num6 * 2), (float)num1, (float)num1);
        //        Widgets.DrawLightHighlight(rect);
        //        Widgets.DrawHighlightIfMouseover(rect);
        //        if (this.newColorDef == colorDef)
        //            Widgets.DrawBox(rect);
        //        Widgets.DrawBoxSolid(new Rect(rect.x + 2f, rect.y + 2f, 22f, 22f), colorDef.color);
        //        if (Widgets.ButtonInvisible(rect))
        //        {
        //            this.newColorDef = colorDef;
        //            SoundDefOf.Tick_High.PlayOneShotOnCamera();
        //        }
        //        curY = Mathf.Max(curY, mainRect.yMin + rect.yMax);
        //        ++num5;
        //    }
        //    GUI.EndGroup();
        //    curY += 4f;
        //}

        private void TryAccept()
        {
            if (!this.buttonMemory.label.NullOrEmpty())
                this.buttonMemory.label = this.buttonMemory.label.Trim();
            this.buttonMemory.Update();
            this.Close();
        }
    }
}
