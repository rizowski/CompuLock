class Ability
  include CanCan::Ability

  def initialize(user)
    # Define abilities for the passed in user here. For example:
    #
    #   user ||= User.new # guest user (not logged in)
    #   if user.admin?
    #     can :manage, :all
    #   else
    #     can :read, :all
    #   end
    #
    # See the wiki for details: https://github.com/ryanb/cancan/wiki/Defining-Abilities
    user ||= User.new 
    if user.admin?
        can :manage, :all
    else
        can :manage, Computer, :user_id => user.id
        can :manage, Account, computer_id: user.computer_ids
        
        can :manage, User, :id => user.id
        can :read, History, computer_id: user.computer_ids

        can :read, AccountProcess, account_id: {computer_id: user.computer_ids}
    end
  end
end
