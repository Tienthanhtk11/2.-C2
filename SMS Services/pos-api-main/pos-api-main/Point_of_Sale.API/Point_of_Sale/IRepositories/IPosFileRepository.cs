using Point_of_Sale.Entities;

namespace Point_of_Sale.IRepositories
{
    public interface IPosFileRepository
    {
        void FileCreate(List<POS_File> file, string table_name, long id, byte type = 1);
        void FileModify(List<POS_File> file, string table_name, long id, byte type = 1);
        void FileSingleCreate(POS_File file, string table_name, long id, byte type = 0);
        void FileSingleModify(POS_File file, string table_name, long id, byte type = 0);

        Task<List<POS_File>> FileList(string table_name, long id);
    }
}
