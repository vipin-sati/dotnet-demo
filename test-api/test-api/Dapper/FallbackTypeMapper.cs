﻿using Dapper;
using System.Reflection;

namespace test_api.Dapper
{
    public class FallbackTypeMapper : SqlMapper.ITypeMap
    {
        private readonly IEnumerable<SqlMapper.ITypeMap> _mappers;

        public FallbackTypeMapper(IEnumerable<SqlMapper.ITypeMap> mappers)
        {
            _mappers = mappers;
        }

        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetMember(columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException nix)
                {
                    // the CustomPropertyTypeMap only supports a no-args
                    // constructor and throws a not implemented exception.
                    // to work around that, catch and ignore.
                }
            }
            return null;
        }
        // implement other interface methods similarly

        // required sometime after version 1.13 of dapper
        public ConstructorInfo FindExplicitConstructor()
        {
            return _mappers
                .Select(mapper => mapper.FindExplicitConstructor())
                .FirstOrDefault(result => result != null);
        }

        ConstructorInfo SqlMapper.ITypeMap.FindConstructor(string[] names, Type[] types)
        {
            throw new NotImplementedException();
        }

        ConstructorInfo SqlMapper.ITypeMap.FindExplicitConstructor()
        {
            throw new NotImplementedException();
        }

        SqlMapper.IMemberMap SqlMapper.ITypeMap.GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            throw new NotImplementedException();
        }

        SqlMapper.IMemberMap SqlMapper.ITypeMap.GetMember(string columnName)
        {
            throw new NotImplementedException();
        }
    }
}
