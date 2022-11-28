using System.Collections;
using System.Collections.Generic;
using TTW.Systems;
using UnityEngine;
using System.Linq;

namespace TTW.Systems{
    public class LinkLibrary : MonoBehaviour
    {
        private List<LinkData> links = new List<LinkData>();

        public void AddLink(string linkId, LinkClass linkClass, ScriptableObject data){
            var matchingLinks = links.Where(l => l.Keyword == linkId);
            if (matchingLinks.Count() > 0) return;

            var linkData = new LinkData(linkId, linkClass, data);
            links.Add(linkData);

            print("added link: " + linkId);
        }

        public LinkData Get(string key){
            return links.FirstOrDefault(l => l.Keyword == key);
        }

        public enum LinkClass{
            Ally,
            Enemy,
            Status,
            Ability,
            Object,
            Command
        }

        public struct LinkData{
            public string Keyword { get; set; }
            public LinkClass LinkClass { get; set; }
            public ActorData ActorData {get; set;}
            public AbilityData AbilityData {get; set;}

            public LinkData(string keyword, LinkClass linkclass, ScriptableObject data){
                Keyword = keyword;
                LinkClass = linkclass;

                ActorData = null;
                AbilityData = null;

                if      
                    (LinkClass == LinkClass.Ally || LinkClass == LinkClass.Enemy) ActorData = (ActorData)data;
                else if 
                    (LinkClass == LinkClass.Ability) AbilityData = (AbilityData)data;
            }
        }
    }
}