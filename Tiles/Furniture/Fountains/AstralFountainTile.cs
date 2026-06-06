using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Localization;
using Terraria.ModLoader;
using CalRD.Dusts;
using CalRD.Items.Placeables.Furniture.Fountains;
using Terraria.DataStructures;

namespace CalRD.Tiles.Furniture.Fountains
{
	public class AstralFountainTile : ModTile
	{
		public override void SetStaticDefaults()
		{
            this.SetUpFountain();
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Astral Water Fountain");
			AddMapEntry(new Color(59, 50, 77), name);
            AnimationFrameHeight = 72;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (!Main.dedServ && Main.tile[i, j].TileFrameX >= 36)
                    Main.SceneMetrics.ActiveFountainColor = ModContent.Find<ModWaterStyle>("CalRD/AstralWater").Slot;
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, ModContent.DustType<AstralBlue>());
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, ModContent.DustType<AstralOrange>());
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frameCounter++;
            if (frameCounter >= 6)
            {
                frame = (frame + 1) % 4;
                frameCounter = 0;
            }
        }

        /*public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 32, ModContent.ItemType<AstralFountainItem>());
        }*/

        public override void HitWire(int i, int j)
        {
            CalamityUtils.LightHitWire(Type, i, j, 2, 4);
        }

        public override bool RightClick(int i, int j)
        {
            CalamityUtils.LightHitWire(Type, i, j, 2, 4);
            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<AstralFountainItem>();
        }
    }
}