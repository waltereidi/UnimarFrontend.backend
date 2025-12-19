using System;
using System.Collections.Generic;
using System.Text;
using UnimarFrontend.backend.Service;
using UnimarFrontend.Tests.Mocks;

namespace UnimarFrontend.Tests.Service
{
    public class BookServiceTest
    {
        private readonly BookService _service;
        
        public BookServiceTest() 
        {
            var context = AppDbContextMock.Create("BookServiceTest");
            _service = new BookService(context);    
            
        }
        [Fact]
        public void GetDateTest()
        {

            var result = _service.GetLastBook();
            Assert.NotNull(result);
        }
    }
}
