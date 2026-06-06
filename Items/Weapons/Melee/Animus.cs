using CalRD.Buffs.DamageOverTime;
using CalRD.Items.Materials;
using CalRD.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class Animus : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Animus");
/*
            Tooltip.SetDefault("Randomizes its damage on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 82;
            Item.height = 84;
			Item.scale = 1.5f;
            Item.damage = 4000;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 11;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 11;
            Item.useTurn = true;
            Item.knockBack = 20f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(5, 0, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Developer;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BladeofEnmity>());
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			float damageMult = player.Calamity().animusBoost;
			//damageMult -= 1f;
			damage *= damageMult;
		}

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 6000);
            int damageRan = Main.rand.Next(195); //0 to 194
            if (damageRan >= 50 && damageRan <= 99) //25%
            {
                player.Calamity().animusBoost = 1.5f;
            }
            else if (damageRan >= 100 && damageRan <= 139) //20%
            {
                player.Calamity().animusBoost = 2.25f;
            }
            else if (damageRan >= 140 && damageRan <= 169) //15%
            {
                player.Calamity().animusBoost = 3.75f;
            }
            else if (damageRan >= 170 && damageRan <= 189) //10%
            {
                player.Calamity().animusBoost = 7.5f;
            }
            else if (damageRan >= 190 && damageRan <= 194) //5%
            {
                player.Calamity().animusBoost = 12.5f;
            }
            else
            {
                player.Calamity().animusBoost = 1f;
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 6000);
            int damageRan = Main.rand.Next(195); //0 to 194
            if (damageRan >= 50 && damageRan <= 99) //25%
            {
                player.Calamity().animusBoost = 1.5f;
            }
            else if (damageRan >= 100 && damageRan <= 139) //20%
            {
                player.Calamity().animusBoost = 2.25f;
            }
            else if (damageRan >= 140 && damageRan <= 169) //15%
            {
                player.Calamity().animusBoost = 3.75f;
            }
            else if (damageRan >= 170 && damageRan <= 189) //10%
            {
                player.Calamity().animusBoost = 7.5f;
            }
            else if (damageRan >= 190 && damageRan <= 194) //5%
            {
                player.Calamity().animusBoost = 12.5f;
            }
            else
            {
                player.Calamity().animusBoost = 1f;
            }
        }
    }
}
