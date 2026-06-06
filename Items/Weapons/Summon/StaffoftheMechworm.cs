using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
	public class StaffoftheMechworm : ModItem
    {
        // This value is also referenced by the God Slayer and Auric summoner helmets.
        public const int BaseDamage = 255; // originally 325
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Staff of the Mechworm");
/*
            Tooltip.SetDefault("Summons an aerial mechworm to fight for you");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = BaseDamage;
            Item.mana = 15;
            Item.width = 58;
            Item.height = 58;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(platinum: 1, gold: 40);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item113;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MechwormHead>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override bool CanUseItem(Player player)
        {
            float neededSlots = 1;
            float foundSlotsCount = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.minion && p.owner == player.whoAmI)
                {
                    foundSlotsCount += p.minionSlots;
                    if (foundSlotsCount + neededSlots > player.maxMinions)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int head = -1;
            int tail = -1;
            for (int num187 = 0; num187 < Main.projectile.Length; num187++)
            {
                if (Main.projectile[num187].active && Main.projectile[num187].owner == Main.myPlayer)
                {
                    if (head == -1 && Main.projectile[num187].type == ModContent.ProjectileType<MechwormHead>())
                    {
                        head = num187;
                    }
                    if (tail == -1 && Main.projectile[num187].type == ModContent.ProjectileType<MechwormTail>())
                    {
                        tail = num187;
                    }
                    if (head != -1 && tail != -1)
                    {
                        break;
                    }
                }
            }
            if (head == -1 && tail == -1)
            {
                int curr = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                curr = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<MechwormBody>(), damage, Item.knockBack, player.whoAmI, Main.projectile[curr].identity, 0f);
                int head2 = curr;
                curr = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<MechwormBody>(), damage, Item.knockBack, player.whoAmI, Main.projectile[curr].identity, 0f);
                Main.projectile[head2].localAI[1] = curr;
                head2 = curr;
                curr = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<MechwormTail>(), damage, Item.knockBack, player.whoAmI, Main.projectile[curr].identity, 0f);
                Main.projectile[head2].localAI[1] = curr;
            }
            else if (head != -1 && tail != -1)
            {
                position = Main.projectile[tail].Center;
                Projectile tailAheadSegment = Main.projectile.Take(Main.maxProjectiles).FirstOrDefault(x => x.identity == (int)Main.projectile[tail].ai[0]);
                int body = Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<MechwormBody>(), damage, Item.knockBack, player.whoAmI, tailAheadSegment.identity, 0f);
                int body2 = body;
                body = Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<MechwormBody>(), damage, Item.knockBack, player.whoAmI, Main.projectile[body].identity, 0f);

                Main.projectile[tail].ai[0] = Main.projectile[body].identity;
                Main.projectile[tail].netUpdate = true;
            }
            return false;
        }
    }
}
