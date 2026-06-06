using CalRD.Items.Materials;
using CalRD.NPCs.Polterghast;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class NecroplasmicBeacon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Necroplasmic Beacon");
/*
            Tooltip.SetDefault("It's spooky\n" +
                "Summons Polterghast\n" +
                "Not consumable");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 58;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/SummonItems/NecroplasmicBeaconGlow").Value);
		}

		public override bool CanUseItem(Player player)
        {
            return player.ZoneDungeon && !NPC.AnyNPCs(ModContent.NPCType<Polterghast>()) && CalamityWorld.downedBossAny;
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Polterghast>());
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<Polterghast>());

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Phantoplasm>(), 100);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
