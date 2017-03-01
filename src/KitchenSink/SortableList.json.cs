using Starcounter;
using System;
using System.Collections.Generic;
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
      
            

            Debug.WriteLine(action);
            if (this.Persons == null)
            {
                return;
            }


            var tmp = Persons[0];
           

            Db.Transact(() =>
            {
                Persons[0].Rank++;
                Persons[1].Rank--;
                Persons[0] = Persons[1];
                Persons[1] = tmp;
                
            });

            //var tmp = Persons[0];
            //Persons[0] = Persons[1];
            //Persons[1] = tmp;
            //Persons[0].Rank++;
            //Persons[1].Rank--;


        }

        void Handle(Input.Down action)
        {
            Debug.WriteLine("hello");
            //get the index of person


            //Transaction.Commit();
        }



    }
}
