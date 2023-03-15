using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Entities;
using Point_of_Sale.IRepositories;

namespace Point_of_Sale.Repositories
{
    internal class PosFileRepository: IPosFileRepository
    {
        private readonly ApplicationContext _context;


        public PosFileRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void FileCreate(List<POS_File> file, string table_name, long id, byte type)
        {
            foreach (var item in file)
            {
                item.tablename = table_name;
                item.idtable = id;
                item.type = type;
            }
            _context.POS_File.AddRange(file);
            _context.SaveChanges();
        }

        public void FileModify(List<POS_File> file, string table_name, long id, byte type)
        {
            foreach (var item in file)
            {
                if (item.id == 0)
                {
                    item.tablename = table_name;
                    item.idtable = id;
                    item.type = type;
                    _context.POS_File.Add(item);
                }
                else
                {
                    _context.Entry(item).State = EntityState.Modified;

                }

            }
            _context.SaveChanges();
        }

        public void FileSingleCreate(POS_File file, string table_name, long id, byte type)
        {

            file.tablename = table_name;
            file.idtable = id;
            file.type = type;

            _context.POS_File.Add(file);
            _context.SaveChanges();
        }
        public void FileSingleModify(POS_File file, string table_name, long id, byte type)
        {
            if (file.id == 0)
            {
                file.tablename = table_name;
                file.idtable = id;
                file.type = type;

                _context.POS_File.Add(file);
            }
            else
            {
                file.type = type;
                _context.Entry(file).State = EntityState.Modified;

            }

            _context.SaveChanges();


        }
        public async Task<List<POS_File>> FileList(string table_name, long id)
        {
            await Task.CompletedTask;
            List<POS_File> file = new List<POS_File>();
            file = _context.POS_File.Where(x => x.tablename == table_name && x.idtable == id && !x.is_delete).ToList();
            return file;
        }
    }
}
