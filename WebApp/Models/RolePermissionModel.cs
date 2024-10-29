namespace WebApp.Models
{
    public class RolePermission
    {
        public int Id { get; set; }

        // Foreign keys
        public string RoleId { get; set; }
        public ApplicationRole Role { get; set; }

        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
