using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtractPDF.Models;
using ExtractPDF.Helper;
namespace ExtractPDF
{
    public class DataAccessService
    {
        private PDFDBContext _context;
        public DataAccessService(PDFDBContext context)
        {
            _context = context;
        }
        // Add Article Entity with Article Name.
        public int AddArticle(string articleName)
        {
            var entry = _context.Article.FirstOrDefault(x => x.Name == articleName);
            if(entry == null)
            {
                entry = new Article
                {
                    Name = articleName
                };
                _context.Article.Add(entry);
                _context.SaveChanges();
            }
            return entry.Id;
        }
        // Add Article which having Modifiers
        public ProcessingArticle AddArticleWithModifiers(ProcessingArticle article)
        {
            var articleEntry = _context.Article.FirstOrDefault(x => x.Name == article.Article);
            
            if (articleEntry == null)
            {
                articleEntry = new Article
                {
                    Name = article.Article
                };
                _context.Article.Add(articleEntry);
                _context.SaveChanges();
            }
            foreach(var item in article.Modifiers)
            {
                var modifierContentEntry = _context.ArticleModifierContent.FirstOrDefault(x => x.Value == item);
                if(modifierContentEntry == null)
                {
                    modifierContentEntry = new ArticleModifierContent()
                    {
                        Value = item
                    };
                    _context.ArticleModifierContent.Add(modifierContentEntry);
                    _context.SaveChanges();
                }

                var modifierEntry = _context.ArticleModifier.FirstOrDefault(x => x.ArticleModifierContentId == modifierContentEntry.Id && x.ArticleId == articleEntry.Id);
                if(modifierEntry == null)
                {
                    modifierEntry = new ArticleModifier()
                    {
                        ArticleId = articleEntry.Id,
                        ArticleModifierContentId = modifierContentEntry.Id
                    };
                    _context.ArticleModifier.Add(modifierEntry);
                    _context.SaveChanges();
                }
            }

            article.Id = articleEntry.Id;

            return article;
        }
        // Add Extend Article.
        public ProcessingArticle AddArticleExtend(ProcessingArticle article)
        {
            if(article.ParentId == 0)
            {
                var parentArticle = _context.Article.FirstOrDefault(x => x.Name == article.Article);
                article.ParentId = parentArticle.Id;
            }

            var articleEntry = _context.Article.FirstOrDefault(x => x.Name == article.Extension && x.ParentId == article.ParentId);
            if(articleEntry == null)
            {
                articleEntry = new Article()
                {
                    Name = article.Extension,
                    ParentId = article.ParentId
                };
                _context.Article.Add(articleEntry);
                _context.SaveChanges();
            }
            
            article.Id = articleEntry.Id;

            return article;
        }
        // Add Association Type.
        public int AddAssociation(int articleId, string type)
        {
            var asContent = _context.AssociationContent.FirstOrDefault(x => x.Value == type);
            if(asContent == null)
            {
                asContent = new AssociationContent()
                {
                    Value = type
                };
                _context.AssociationContent.Add(asContent);
                _context.SaveChanges();
            }
            Association association = new Association()
            {
                ArticleId = articleId,
                AssociationContentId = asContent.Id
            };
            _context.Association.Add(association);
            _context.SaveChanges();

            return association.Id;
        }
        // Add Association Value.
        public int AddAssociationValue(int associationId, string value)
        {
            var asValueContent = _context.AssociationValueContent.FirstOrDefault(x => x.Value == value);
            if (asValueContent == null)
            {
                asValueContent = new AssociationValueContent()
                {
                    Value = value
                };
                _context.AssociationValueContent.Add(asValueContent);
                _context.SaveChanges();
            }
            AssociationValue associationValue = new AssociationValue()
            {
                AssociationId = associationId,
                AssociationValueContentId = asValueContent.Id
            };
            _context.AssociationValue.Add(associationValue);
            _context.SaveChanges();

            return associationValue.Id;
        }
        // Add Association Value Modifier.
        public int AddAssociationValueModifier(int associationValueId, string value)
        {
            var asValueModifierContent = _context.AssociationValueModifierContent.FirstOrDefault(x => x.Value == value);
            if (asValueModifierContent == null)
            {
                asValueModifierContent = new AssociationValueModifierContent()
                {
                    Value = value
                };
                _context.AssociationValueModifierContent.Add(asValueModifierContent);
                _context.SaveChanges();
            }
            AssociationValueModifier associationValueModifier = new AssociationValueModifier()
            {
                AssociationValueId = associationValueId,
                AssociationValueModifierContentId = asValueModifierContent.Id
            };
            _context.AssociationValueModifier.Add(associationValueModifier);
            _context.SaveChanges();

            return associationValueModifier.Id;
        }
    }
}
