using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentProjectManagementAPI.Models;

namespace StudentProjectManagementAPI.Data
{
    public class TeamMemberRepository
    {
        public readonly SpmContext _context;
        #region TeamMemberRepository
        public TeamMemberRepository(SpmContext _context)
        {
            this._context = _context;
        }
        #endregion

        #region  TeamMemberList
        public async Task<List<TeamMember>> TeamMemberList()
        {
            var teamMemberList = await _context.TeamMembers.Include(s => s.Student).Include(s => s.Project).ToListAsync();
            return teamMemberList;
        }
        #endregion

        #region GetTeamMemberById
        public async Task<TeamMember> GetStudentById(int teamMemberId)
        {
            TeamMember teamMember = await _context.TeamMembers.Include(s => s.Student).Include(s => s.Project).FirstOrDefaultAsync(s => s.TeamMemberId == teamMemberId);
            if (teamMember == null)
            {
                return null;
            }
            return teamMember;
        }
        #endregion

        #region AddTeamMember
        public async Task<bool> AddTeamMember(TeamMember teamMember)
        {
            _context.TeamMembers.Add(teamMember);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region UpdateTeamMember
        public async Task<bool> UpdateTeamMember(TeamMember teamMember)
        {
            _context.TeamMembers.Update(teamMember);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region DeleteTeamMember
        public async Task<bool> DeleteTeamMember(int teamMemberId)
        {
            var teamMember = await _context.TeamMembers.FindAsync(teamMemberId);
            if (teamMember == null)
            {
                return false;
            }
            _context.TeamMembers.Remove(teamMember);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region saveTeamMembers
        public async Task<bool> saveTeamMembers(int projectId,string membersIds)
        {

            var data = new List<TeamMember>();
            foreach (var memberId in membersIds.Split(","))
            {
                data.Add(new TeamMember()
                {
                    ProjectId = projectId,
                    StudentId = Convert.ToInt32(memberId),
                });
            }


            _context.TeamMembers.AddRangeAsync(data);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region UpdateTeamMembers
        public async Task<bool> UpdateTeamMembers(int projectId, string membersIds)
        {
            Console.WriteLine(":::::::::::::::::::::::: "+projectId+" :::::::::::::::::: "+membersIds);
            var dataFromDB = await _context.TeamMembers.Where(x => x.ProjectId == projectId).ToListAsync();
            var dataFromUI = new List<TeamMember>();
            var data = new List<TeamMember>();
            foreach (var memberId in membersIds.Split(","))
            {
                dataFromUI.Add(new TeamMember()
                {
                    ProjectId = projectId,
                    StudentId = Convert.ToInt32(memberId),
                });
            }

            //add if uidata have member but dbdata not
            foreach (var member in dataFromUI)
            {
                if(!dataFromDB.Any(x => x.StudentId == member.StudentId))
                {
                    try
                    {
                        _context.TeamMembers.Add(member);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }

            //remove if dbdata have member but uidata not
            foreach (var member in dataFromDB)
            {
                if (!dataFromUI.Any(x => x.StudentId == member.StudentId))
                {
                    try
                    {
                        _context.TeamMembers.Remove(member);
                        _context.SaveChangesAsync();
                    }
                    catch (Exception ex) 
                    { 
                        return false;
                    }
                }
            }
            return true;
            

        }
        #endregion
    }
}
