using DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogsServer.Domain.SearchCriteria
{
    public class ReviewSearchCriteria
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool MatchesCriteria(Log logReview)
        {
            bool matchesCriteria = false;

            if (logReview.EntityType == typeof(ReviewDetailDTO) || logReview.EntityType == typeof(List<ReviewDetailDTO>))
            {
                matchesCriteria = MatchesId(logReview) && MatchesDescription(logReview) && MatchesName(logReview) &&
                       MatchesLogTag(logReview);
            }

            return matchesCriteria;
        }

        private bool MatchesId(Log logReview)
        {
            bool matchesId = true;

            if (Id != null)
            {
                if (logReview.IsEntityAList())
                {
                    List<ReviewDetailDTO> review = logReview.Entity as List<ReviewDetailDTO>;
                    //TODO    matchesId = review.Any(t => t.Id == Id);
                }
                else
                {
                    ReviewDetailDTO review = logReview.Entity as ReviewDetailDTO;
                    //TODO    matchesId = review.Id == Id;
                }
            }

            return matchesId;
        }

        private bool MatchesName(Log logReview)
        {
            bool matchesName = true;

            if (!String.IsNullOrEmpty(Name))
            {
                if (logReview.IsEntityAList())
                {
                    List<ReviewDetailDTO> reviews = logReview.Entity as List<ReviewDetailDTO>;
                   matchesName = reviews.Any(t => t.Description.ToLower().Contains(Description.ToLower()));
             //Aca puede ser Id TODO
                }
                else
                {
                    ReviewDetailDTO review = logReview.Entity as ReviewDetailDTO;
                    matchesName = review.Description.ToLower().Contains(Description.ToLower());
                    //Aca puede ser Id TODO
                }
            }

            return matchesName;
        }

        private bool MatchesDescription(Log logReview)
        {
            bool matchesDescription = true;

            if (!String.IsNullOrEmpty(Description))
            {
                if (logReview.IsEntityAList())
                {
                    List<ReviewDetailDTO> reviews = logReview.Entity as List<ReviewDetailDTO>;
                    matchesDescription = reviews.Any(t => t.Description.ToLower().Contains(Description.ToLower()));
                }
                else
                {
                    ReviewDetailDTO review = logReview.Entity as ReviewDetailDTO;
                    matchesDescription = review.Description.ToLower().Contains(Description.ToLower());
                }
            }

            return matchesDescription;
        }


        private bool MatchesLogTag(Log logReview)
        {
            return logReview.LogTag == LogTag.CreateReview || logReview.LogTag == LogTag.DeleteReview ||
                   logReview.LogTag == LogTag.UpdateReview || logReview.LogTag == LogTag.IndexReview;
        }
    }
}
