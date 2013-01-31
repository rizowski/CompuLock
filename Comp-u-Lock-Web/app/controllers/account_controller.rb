class AccountController < ApplicationController
  before_filter :authenticate_user!#, :except => [:index, :list]
  load_and_authorize_resource
  # View
  def index
  	list
  	render("list")
  end
  # View
  def show

  end
  # View
  def list
    @computers = current_user.computer.all
  end

  # form
  def edit
    @account = Account.find(params[:id])
    unless can_edit? @account
      @account
    else
      flash[:error] = "Sorry but this cannot be edited with your level account. If this is an issue contact the Admin."
      redirect_to(request.referer)
    end
  end

  
  # Put
  def update
    @account = Account.find(params[:id])
  end

  # Put
  def save
    @account.(params[:account])
  end

  private
  def can_edit? item
    current_user.computer.all.each do |computer|
      return if item.computer_id == computer.id
    end
    return false
  end
end
