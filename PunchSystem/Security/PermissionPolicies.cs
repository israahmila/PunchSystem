namespace PunchSystem.Security
{
    public static class PermissionPolicies
    {
        // 🧑‍💼 User Management
        public const string ManageUsers = "User.Manage";
        public const string ManageRoles = "Role.Manage";

        // 📦 Produits
        public const string ManageProduits = "Produit.Manage";

        // 🛠 Poinçons
        public const string ManagePoincons = "Poincon.Manage";

        // 🏭 Fournisseurs
        public const string ManageFournisseurs = "Fournisseur.Manage";

        // 🏷 Marques
        public const string ManageMarques = "Marque.Manage";

        // 🧪 Utilisations
        public const string CreateUtilisation = "Utilisation.Create";

        // 📊 Statistiques
        public const string ViewStats = "Stats.View";

        // 📘 Audit
        public const string ViewAudit = "Audit.View";
    }
}
