class ComputerAccountsController < ApplicationController
  before_filter :authenticate_user!#, :except => [:index, :list]

  def index
  	list
  end

  def edit
  end

  def list
  		user = User.find(current_user.id)
  		@computers = user.computer.all
  end

  def update

  end

  def save

  end

  def show

  end
end
