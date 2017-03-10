using Starcounter;
using System;
using System.Diagnostics;

namespace KitchenSink
{
    [Database]
    public class Person
    {
        public string FirstName;
        public string LastName;
        public int Rank;

        public bool isLast {
            get {
                Int64 count = Db.SlowSQL<Int64>("select count(*) from Person").First;
                return count==(Rank+1);
            }
        }
        public bool isFirst
        {
            get
            {
                return Rank == 0;
            }
        }
    }


    partial class SortableList : Json
    {

        public void RefreshData()
        {
          
            Persons = Db.SQL<Person>("SELECT p FROM Person p ORDER BY p.Rank");
            

        }
        protected override void OnData()
        {
            base.OnData();
            Db.Transact(() =>
            {
                var anyone = Db.SQL<Person>("SELECT p FROM Person p").First;
                if (anyone == null)
                {
                    new Person
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        Rank = 0
                    };
                    new Person
                    {
                        FirstName = "test",
                        LastName = "user",
                        Rank = 1
                    };
                    new Person
                    {
                        FirstName = "jawher",
                        LastName = "bouguila",
                        Rank = 2
                    };
                    new Person
                    {
                        FirstName = "star",
                        LastName = "counter",
                        Rank = 3
                    };
                }
            });

            var person = Db.SQL<Person>("SELECT p FROM Person p ORDER BY p.Rank");
            Persons.Data = person;
        }

        void Handle(Input.Up action)
        {

            int idx = (int)this.index;

            if (this.Persons == null)
            {
                return;
            }

            var tmp = Persons[idx-1];
           
            Db.Transact(() =>
            {
                Persons[idx-1].Rank++;
                Persons[idx].Rank--;
                Persons[idx-1] = Persons[idx];
                Persons[idx] = tmp;
                
            });
            this.RefreshData();


        }

        void Handle(Input.Down action)
        {
            int idx = (int)this.index;

            Debug.WriteLine(idx);
            if (this.Persons == null)
            {
                return;
            }

            var tmp = Persons[idx+1];


            Db.Transact(() =>
            {
                Persons[idx+1].Rank--;
                Persons[idx].Rank++;
                Persons[idx+1] = Persons[idx];
                Persons[idx] = tmp;

            });
        }

        void Handle(Input.Drag action)
        {
            int from =((int)this.index-1) ;
            int to = ((int)this.to-1);
            
            Debug.WriteLine(from+" ===> "+to);
            if (this.Persons == null)
            {
                return;
            }


            var tmpRank = Persons[from].Rank;
            var tmp = Persons[from];
            Db.Transact(() =>
            {
                Persons[from].Rank=Persons[to].Rank;
                Persons[to].Rank=tmpRank;
                Persons[from] = Persons[to];
                Persons[to] = tmp;

            });
        }

    }
}
