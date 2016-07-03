using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DapperQ;


namespace Tester
{
    class Program
    {
        static DapperQ<User> p1;
        static DapperQ<Image> p2;
        static DapperQ<Message> p3;
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            dynamic result = null;

            p2 = new DapperQ<Image>();
            var info = new Image
            {
                Name = "Good Man",
                Url = "http://localhost",
                CreateOn = DateTime.Now,
                IsOpen = 1,
                Path = "c:\\",
                Type = 1,
                Level = 1
            };
            p2.Insert(info);
            p2.Dispose();
            Console.WriteLine("success to insert");
            Console.WriteLine("-----------------------------------------------------------------------------");





            p1 = new DapperQ<User>();
            var mq = p1.AsQueryable().Where(b => b.Id > 0);
            Console.WriteLine(mq.ToString());
            result = mq.ToList();
            Console.WriteLine("result count: " + result.Count);
            p1.Dispose();
            Console.WriteLine("-----------------------------------------------------------------------------");




            p1 = new DapperQ<User>();
            var mq1 = from b in p1.AsQueryable() where b.IsOpen != false select b;
            var sq1 = mq1.Where(b => b.Id > 0);
            Console.WriteLine(sq1.ToString());
            result = sq1.ToList();
            Console.WriteLine("result count: " + result.Count);
            p1.Dispose();
            Console.WriteLine("-----------------------------------------------------------------------------");





            p1 = new DapperQ<User>();
            var q1 = (from a in p1.AsQueryable() where a.Level == 1 && a.Type == 1 && a.Password == "" select a)
                .Union
                (from b in p1.AsQueryable() where (b.Level > 1 && b.Type == 2) || b.IsOpen != true select b);

            Console.WriteLine(q1.ToString());
            result = q1.ToList();
            Console.WriteLine("result count: " + result.Count);
            p1.Dispose();
            Console.WriteLine("-----------------------------------------------------------------------------");




            p1 = new DapperQ<User>();
            result = p1.AsQueryable().First();
            if (result == null)
                Console.WriteLine("can not find data by first.");
            else
                Console.WriteLine("first id: " + result.Id);
            p1.Dispose();
            Console.WriteLine("-----------------------------------------------------------------------------");



            p1 = new DapperQ<User>();
            result = p1.AsQueryable().Exists();
            if (result == true)
                Console.WriteLine("datas is exists.");
            else
                Console.WriteLine("data do no exists.");
            p1.Dispose();
            Console.WriteLine("-----------------------------------------------------------------------------");


            p2 = new DapperQ<Image>();
            var pageQuery = p2.AsQueryable().Where(o => o.Level == 1).Where(o => o.IsOpen == 1).Take(20).Skip(10);
            Console.WriteLine(pageQuery.ToString());
            result = pageQuery.ToList();
            Console.WriteLine("result count: " + result.Count);
            p2.Dispose();
            Console.WriteLine("-----------------------------------------------------------------------------");



            p2 = new DapperQ<Image>();
            var q3 = from c in p2.AsQueryable() where c.Id > 0 && c.Name.Contains("Good") select new { c.Id, c.Name, c.Path, c.Url, c.UserId };
            Console.WriteLine(q3.ToString());
            result = q3.ToList();
            Console.WriteLine("result count: " + result.Count);
            p2.Dispose();
            Console.WriteLine("-----------------------------------------------------------------------------");


            p1 = new DapperQ<User>();
            p2 = new DapperQ<Image>();
            var q4 = from d in p1.AsQueryable()
                     join e in p2.AsQueryable() on d.Id equals e.UserId into ords
                     from f in ords.DefaultIfEmpty()
                     where d.IsOpen == true
                     select new { d.Name, d.Level };

            Console.WriteLine(q4.ToString());

            result = q4.ToList();
            Console.WriteLine("result count: " + result.Count);
            p1.Dispose();
            p2.Dispose();
            Console.WriteLine("-----------------------------------------------------------------------------");



            p1 = new DapperQ<User>();
            p2 = new DapperQ<Image>();
            p3 = new DapperQ<Message>();
            var q5 = from g in p1.AsQueryable()
                     join h in p2.AsQueryable() on g.Id equals h.Id
                     join i in p3.AsQueryable() on g.Id equals i.UserId
                     where h.IsOpen == 1
                     select new { g.Name, h.Path, i.Content };

            Console.WriteLine(q5.ToString());
            result = q5.ToList();
            Console.WriteLine("result count: " + result.Count);
            p1.Dispose();
            p2.Dispose();
            p3.Dispose();
            Console.WriteLine("-----------------------------------------------------------------------------");



            try
            {
                p2 = new DapperQ<Image>();
                p2.CreateIndex("Name");
                Console.WriteLine("success to create index");
                Console.WriteLine("-----------------------------------------------------------------------------");
                p2.Dispose();

                p2 = new DapperQ<Image>();
                p2.DropIndex("Name");
                Console.WriteLine("success to drop index");
                Console.WriteLine("-----------------------------------------------------------------------------");
                p2.Dispose();

                p2 = new DapperQ<Image>();
                var isExists = p2.Exists(o => o.IsOpen == 1);
                if (isExists)
                    Console.WriteLine("exists record");
                else
                    Console.WriteLine("no exists record");
                Console.WriteLine("-----------------------------------------------------------------------------");
                p2.Dispose();

                p2 = new DapperQ<Image>();
                var indexNames = p2.GetIndexes();
                if (indexNames == null)
                    Console.WriteLine("'Image' has not index.");
                else
                    Console.WriteLine(string.Format("index names '{0}' for 'Image'.", string.Join("', '", indexNames)));
                Console.WriteLine("-----------------------------------------------------------------------------");
                p2.Dispose();



                p2 = new DapperQ<Image>();
                var flag = p2.IndexExistsByName("Name");
                if (flag)
                    Console.WriteLine("exists index");
                else
                    Console.WriteLine("no exists index");
                p2.Dispose();
                Console.WriteLine("-----------------------------------------------------------------------------");



                //p2 = new DapperQ<Image>();
                //p2.Drop();
                //p2.Dispose();


                p2 = new DapperQ<Image>();
                var q7 = from c in p2.AsQueryable() where c.Name.Contains("good") select new { c.Name, c.UserId, c.CreateOn, c.IsOpen, c.Level, c.Path, c.Type, c.Url };
                Console.WriteLine(q7.ToString<Photo>(DapperQ.Action.InsertSelect));
                q7.Insert<Photo>();
                Console.WriteLine("success to insert from select");
                p2.Dispose();
                Console.WriteLine("-----------------------------------------------------------------------------");



                p2 = new DapperQ<Image>();
                var q8 = from c in p2.AsQueryable() where c.Name.Contains("good") select new { Level = c.Id, Name = c.Name };
                Console.WriteLine(q8.ToString(DapperQ.Action.UpdateSelect, o => new { Level = 1, Name = "test" }));
                q8.Update(o => new { Level = 1, Name = "test" });
                Console.WriteLine("success to update from select");
                Console.WriteLine("-----------------------------------------------------------------------------");
                p2.Dispose();


                p1 = new DapperQ<User>();
                var entity = new User
                {
                    Id = 11,
                    CreateOn = DateTime.Now,
                    Name = "Rocky",
                    Password = "12345",
                    IsOpen = true
                };
                p1.AsQueryable().Where(o => o.Id == 11).Update(entity);
                Console.WriteLine("success to update from select");
                Console.WriteLine("-----------------------------------------------------------------------------");
                p1.Dispose();


                p2 = new DapperQ<Image>();
                var q9 = from c in p2.AsQueryable() where c.Name.Contains("good") select c;
                Console.WriteLine(q9.ToString(DapperQ.Action.DeleteSelect));
                q9.Delete();
                Console.WriteLine("success to delete from select");
                p2.Dispose();
                Console.WriteLine("-----------------------------------------------------------------------------");



                p1 = new DapperQ<User>();
                var id = "11";
                Expression<Func<User, bool>> exp = o => o.Id == Pase<int>(id);
                var data = p1.AsQueryable().Where(exp).First();
                var count = p1.AsQueryable().Where(exp).Count();
                Console.WriteLine("success to find first data, total is " + count.ToString() + ".");
                Console.WriteLine("-----------------------------------------------------------------------------");
                p1.Dispose();


                p2 = new DapperQ<Image>();
                p2.Update(o => new Image { Url = "root", Name = "a.jpg" }, o => o.Name == string.Empty);
                Console.WriteLine("success to update by lambda");
                Console.WriteLine("-----------------------------------------------------------------------------");
                p2.Dispose();


                p1 = new DapperQ<User>();
                var keyword = "root";
                Expression<Func<User, bool>> exp1 = o => o.Name.Contains(keyword);
                result = p1.AsQueryable().Where(exp1).ToList();
                Console.WriteLine("success to delete by lambda");
                Console.WriteLine("-----------------------------------------------------------------------------");
                p1.Dispose();



                p2 = new DapperQ<Image>();
                var q10 = (from c in p2.AsQueryable()
                           where c.Name.Contains("good")
                           select new { Level = c.IsOpen, Name = c.Name })
                          .Distinct();
                Console.WriteLine(q10.ToString(DapperQ.Action.Select, o => new { Level = 1, Name = "test" }));
                result = q10.ToList();
                Console.WriteLine("distinct result count: " + result.Count);
                Console.WriteLine("-----------------------------------------------------------------------------");
                p2.Dispose();


                p2 = new DapperQ<Image>();
                var q11 = from c in p2.AsQueryable()
                          where c.Name.Contains("good")
                          select new { Id = c.Id.Max(), Level = c.Level.Sum(), IsOpen = c.IsOpen.Average() };
                Console.WriteLine(q11.ToString(DapperQ.Action.Select));
                result = q11.ToList();
                Console.WriteLine("Max Id: {0}, Sum Level: {1}, Average IsOpen: {2}", result[0].Id, result[0].Level, result[0].IsOpen);
                Console.WriteLine("-----------------------------------------------------------------------------");
                p2.Dispose();


                p2 = new DapperQ<Image>();
                var q12 = from c in p2.AsQueryable()
                          where c.Name.Contains("good")
                          select c;
                Console.WriteLine("Level Count: " + q12.Count());
                Console.WriteLine("-----------------------------------------------------------------------------");
                p2.Dispose();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("-----------------------------------------------------------------------------");
            }

            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            Console.WriteLine(string.Format("times: {0}ms", timeSpan.Milliseconds));

            Console.ReadLine();
        }

        private static int Pase<T1>(string id)
        {
            return int.Parse(id);
        }
    }
}
