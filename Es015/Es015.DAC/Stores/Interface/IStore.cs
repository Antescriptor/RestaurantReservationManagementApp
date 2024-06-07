using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es016.DAL.Stores.Interface
{
    internal interface IStore<T>
    {
        public bool Add(T item);
        public bool Delete(uint id );
        public List<T>? Get();
        public T? Get(uint id);
        public bool Update(T item);
	}
}
