﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniPascal.FrontEnd.LexicalAnalysis;
using MiniPascal.FrontEnd.Parsing;
using MiniPascal.FrontEnd.SemanticAnalysis;
using MiniPascal.Utils.Source;
using MiniPascal.Utils;
using MiniPascal.ErrorHandling;
using MiniPascal.BackEnd;

namespace MiniPascal
{
    class Program
    {
        static void Main(string[] args)
        {
            ErrorManager em = new ErrorManager();
            string path = "TestPrograms/test.mpas";
            Lexer lexer = new Lexer(new FileSource(path));

            Parser parser = new Parser(lexer);
            parser.HookTo(em);
            AST program = parser.parse();

            SymbolTableBuildingVisitor tableBuilder = new SymbolTableBuildingVisitor();
            tableBuilder.HookTo(em);
            tableBuilder.buildTables(program);

            if (!em.areErrors())
            {
                CodeGeneratingVisitor cgv = new CodeGeneratingVisitor(program);
                CProgram cProgram = cgv.generate();
                cProgram.generateFile();
            }
            else
            {
                Console.WriteLine("Build failed.");
            }
            
            Console.ReadLine();
        }
    }
}
