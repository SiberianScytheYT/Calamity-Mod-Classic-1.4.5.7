using CalRD.Buffs.StatDebuffs;
using CalRD.CalPlayer;
using CalRD.Items.Materials;
using CalRD.Items.Weapons.Melee;
using CalRD.Projectiles.Summon;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class PrototypeAndromechaRing : ModItem
    {
        public const int CrippleTime = 360; // 6 seconds
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Flamsteed Ring");
/*
            Tooltip.SetDefault("Summons a colossal controllable mech\n" +
			"Right click to display the mech's control panel\n" +
			"The panel has 3 configurations, selected using the brackets on the edges of the UI\n" +
			"Each bracket powers 2 out of 3 possible functions, represented by the circular icons.\n" +
			"The bottom left icon miniaturizes the mech to the size of a player, but weakens its weapons.\n" +
			"The bottom right icon is a powerful jet booster which greatly enhances movement.\n" +
			"The top icon is the mech's weaponry. It must be powered in order to attack.\n" +
			"Click the top icon to switch between Regicide, an enormous energy blade, and a powerful Gauss rifle.\n" +
			"Exiting the mount while a boss is alive will temporarily hinder your movement\n" +
			CalamityUtils.ColorMessage("Now, make them pay.", new Color(135, 206, 235)));
*/
        }

        public override void SetDefaults()
        {
            Item.mana = 200;
            Item.damage = 9999;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.width = Item.height = 28;
            Item.useTime = Item.useAnimation = 10;
            Item.noMelee = true;
            Item.knockBack = 1f;
            Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
            Item.rare = 10;
            Item.UseSound = SoundID.Item117;
            Item.shoot = ModContent.ProjectileType<GiantIbanRobotOfDoom>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<Excelsus>(), 4);
            recipe.AddIngredient(ModContent.ItemType<CosmicViperEngine>());
            recipe.AddIngredient(ItemID.WingsVortex);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }

        public override bool CanUseItem(Player player) => !(player.Calamity().andromedaCripple > 0 && CalamityPlayer.areThereAnyDamnBosses);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // If the player has any robots, kill them all.
            if (player.ownedProjectileCounts[Item.shoot] > 0)
            {
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].active && 
                        Main.projectile[i].type == Item.shoot &&
                        Main.projectile[i].owner == player.whoAmI)
                    {
                        Main.projectile[i].Kill();
                    }
                }
                if (CalamityPlayer.areThereAnyDamnBosses)
                {
                    player.Calamity().andromedaCripple = CrippleTime;
                    player.AddBuff(ModContent.BuffType<AndromedaCripple>(), player.Calamity().andromedaCripple);
                    SoundEngine.PlaySound(new SoundStyle("CalRD/Sounds/Custom/AdrenalineBurnout1"), position);
                }
                return false;
            }
            // Otherwise create one.
            return true;
        }
    }
}
