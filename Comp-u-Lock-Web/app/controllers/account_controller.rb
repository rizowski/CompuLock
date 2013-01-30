class AccountController < ApplicationController
  before_filter :authenticate_user!#, :except => [:index, :list]
  def index
  end

  def edit
  end

  def list
  	@accounts = Account.all
  	if can :read, @accounts
  		@accounts
  	else
  		redirect_to :action => "index"
  	end
  end

  def update

  end

  def save

  end

  def show

  end
end
